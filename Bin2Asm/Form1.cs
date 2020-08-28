using System;
using System.IO;
using System.Windows.Forms;

namespace Bin2Asm {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void GenereAsm(byte[] tabByte, int start, int length, int calcStart, int startReel, int exe, string fullName) {
			using (StreamWriter sw = new StreamWriter(fullName)) {
				if (startReel != -1 && exe != -1) {
					sw.WriteLine("	ORG	#" + calcStart.ToString("X4"));
					sw.WriteLine("");
				}
				sw.WriteLine("DataPacked:");
				string line = "\tDB\t";
				int nbOctets = 0;
				for (int i = start; i < length; i++) {
					line += "#" + tabByte[i].ToString("X2") + ",";
					if (++nbOctets >= 16) {
						sw.WriteLine(line.Substring(0, line.Length - 1));
						line = "\tDB\t";
						nbOctets = 0;
					}
				}
				if (nbOctets > 0)
					sw.WriteLine(line.Substring(0, line.Length - 1));
				if (startReel != -1 && exe != -1)
					GenereDepack(sw, startReel, exe);
			}
		}

		private void GenereDepack(StreamWriter sw, int start, int jump) {
			sw.WriteLine("; Decompactage");

			sw.WriteLine("	RUN $");
			sw.WriteLine("start:");
			sw.WriteLine("	LD	HL,DataPacked");
			sw.WriteLine("	LD	DE,#" + start.ToString("X4"));

			sw.WriteLine("DepkLzw:");
			sw.WriteLine("	LD	A,(HL)			; DepackBits = InBuf[ InBytes++ ]");
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	RRA				; Rotation rapide calcul seulement flag C");
			sw.WriteLine("	SET	7,A			; Positionne bit 7 en gardant flag C");
			sw.WriteLine("	LD	(BclLzw+1),A");
			sw.WriteLine("	JR	C,TstCodeLzw");
			sw.WriteLine("CopByteLzw:");
			sw.WriteLine("	LDI				; OutBuf[ OutBytes++ ] = InBuf[ InBytes++ ]" + Environment.NewLine);
			sw.WriteLine("BclLzw:");
			sw.WriteLine("	LD	A,0");
			sw.WriteLine("	RR	A			; Rotation avec calcul Flags C et Z");
			sw.WriteLine("	LD	(BclLzw+1),A");
			sw.WriteLine("	JR	NC,CopByteLzw");
			sw.WriteLine("	JR	Z,DepkLzw" + Environment.NewLine);
			sw.WriteLine("TstCodeLzw:");
			sw.WriteLine("	LD	A,(HL)			; A = InBuf[ InBytes ];");
			sw.WriteLine("	AND	A");
			sw.WriteLine("	JP	Z,#" + jump.ToString("X4"));
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	LD	B,A			; B = InBuf[ InBytes ]");
			sw.WriteLine("	RLCA				; A & #80 ?");
			sw.WriteLine("	JR	NC,TstLzw40" + Environment.NewLine);
			sw.WriteLine("	RLCA");
			sw.WriteLine("	RLCA");
			sw.WriteLine("	RLCA");
			sw.WriteLine("	AND	7");
			sw.WriteLine("	ADD	A,3			; Longueur = 3 + ( ( InBuf[ InBytes ] >> 4 ) & 7 );");
			sw.WriteLine("	LD	C,A			; C = Longueur");
			sw.WriteLine("	LD	A,B			; B = InBuf[InBytes]");
			sw.WriteLine("	AND	#0F			; Delta = ( InBuf[ InBytes++ ] & 15 ) << 8");
			sw.WriteLine("	LD	B,A			; B = poids fort Delta");
			sw.WriteLine("	LD	A,C			; A = Length");
			sw.WriteLine("	SCF				; Repositionner flag C (pour Delta++)");
			sw.WriteLine("CopyBytes0:");
			sw.WriteLine("	LD	C,(HL)			; C = poids faible Delta (Delta |= InBuf[ InBytes++ ]);");
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	PUSH	HL");
			sw.WriteLine("	LD	H,D");
			sw.WriteLine("	LD	L,E");
			sw.WriteLine("	SBC	HL,BC			; HL=HL-(BC+1)");
			sw.WriteLine("	LD	B,0");
			sw.WriteLine("CopyBytes1:");
			sw.WriteLine("	LD	C,A");
			sw.WriteLine("CopyBytes2:");
			sw.WriteLine("	LDIR");
			sw.WriteLine("CopyBytes3:");
			sw.WriteLine("	POP	HL");
			sw.WriteLine("	JR	BclLzw" + Environment.NewLine);
			sw.WriteLine("TstLzw40:");
			sw.WriteLine("	RLCA				; A & #40 ?");
			sw.WriteLine("	JR	NC,TstLzw20" + Environment.NewLine);
			sw.WriteLine("	LD	C,B");
			sw.WriteLine("	RES	6,C			; Delta = 1 + InBuf[ InBytes++ ] & #3f;");
			sw.WriteLine("	LD	B,0			; BC = Delta + 1 car flag C = 1");
			sw.WriteLine("	PUSH	HL");
			sw.WriteLine("	LD	H,D");
			sw.WriteLine("	LD	L,E");
			sw.WriteLine("	SBC	HL,BC");
			sw.WriteLine("	LDI");
			sw.WriteLine("	LDI				; Longueur = 2");
			sw.WriteLine("	JR	CopyBytes3" + Environment.NewLine);
			sw.WriteLine("TstLzw20:");
			sw.WriteLine("	RLCA				; A & #20 ?");
			sw.WriteLine("	JR	NC,TstLzw10" + Environment.NewLine);
			sw.WriteLine("	LD	A,B			; B compris entre #20 et #3F");
			sw.WriteLine("	ADD	A,#E2			; = ( A AND #1F ) + 2, et positionne carry");
			sw.WriteLine("	LD	B,0			; Longueur = 2 + ( InBuf[ InBytes++ ] & 31 );");
			sw.WriteLine("	JR	CopyBytes0" + Environment.NewLine);
			sw.WriteLine("CodeLzw0F:");
			sw.WriteLine("	LD	C,(HL)");
			sw.WriteLine("	PUSH	HL");
			sw.WriteLine("	LD	H,D");
			sw.WriteLine("	LD	L,E");
			sw.WriteLine("	CP	#F0");
			sw.WriteLine("	JR	NZ,CodeLzw02" + Environment.NewLine);
			sw.WriteLine("	XOR	A");
			sw.WriteLine("	LD	B,A");
			sw.WriteLine("	INC	BC			; Longueur = Delta = InBuf[ InBytes + 1 ] + 1;");
			sw.WriteLine("	SBC	HL,BC");
			sw.WriteLine("	LDIR");
			sw.WriteLine("	POP	HL");
			sw.WriteLine("	INC	HL			; Inbytes += 2");
			sw.WriteLine("	JR	BclLzw" + Environment.NewLine);
			sw.WriteLine("CodeLzw02:");
			sw.WriteLine("	CP	#20");
			sw.WriteLine("	JR	C,CodeLzw01" + Environment.NewLine);
			sw.WriteLine("	LD	C,B			; Longueur = Delta = InBuf[ InBytes ];");
			sw.WriteLine("	LD	B,0");
			sw.WriteLine("	SBC	HL,BC");
			sw.WriteLine("	JR	CopyBytes2" + Environment.NewLine);
			sw.WriteLine("CodeLzw01:				; Ici, B = 1");
			sw.WriteLine("	XOR	A			; Carry a zéro");
			sw.WriteLine("	DEC	H			; Longueur = Delta = 256");
			sw.WriteLine("	JR	CopyBytes1" + Environment.NewLine);
			sw.WriteLine("TstLzw10:");
			sw.WriteLine("	RLCA				; A & #10 ?");
			sw.WriteLine("	JR	NC,CodeLzw0F" + Environment.NewLine);
			sw.WriteLine("	RES	4,B			; B = Delta(high) -> ( InBuf[ InBytes++ ] & 15 ) << 8;");
			sw.WriteLine("	LD	C,(HL)			; C = Delta(low)  -> InBuf[ InBytes++ ];");
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	LD	A,(HL)			; A = Longueur - 1");
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	PUSH	HL");
			sw.WriteLine("	LD	H,D");
			sw.WriteLine("	LD	L,E");
			sw.WriteLine("	SBC	HL,BC			; Flag C=1 -> hl=hl-(bc+1) (Delta+1)");
			sw.WriteLine("	LD	B,0");
			sw.WriteLine("	LD	C,A");
			sw.WriteLine("	INC	BC			; BC =  Longueur = InBuf[ InBytes++ ] + 1;");
			sw.WriteLine("	JR	CopyBytes2" + Environment.NewLine);
		}

		private void bpImport_Click(object sender, EventArgs e) {
			OpenFileDialog dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == DialogResult.OK) {
				byte[] tabByte = File.ReadAllBytes(dlg.FileName);
				bool isAmsdos = CpcSystem.CheckAmsdos(tabByte);
				int exe = -1, startReel = -1;

				if (isAmsdos) {
					CpcAmsdos enteteAms = CpcSystem.GetAmsdos(tabByte);
					startReel = enteteAms.Adress;
					exe = enteteAms.EntryAdress;
				}
				int start = chkAmsdos.Checked && isAmsdos ? 0x80 : 0;
				int length = tabByte.Length - start;
				if (chkPack.Checked) {
					byte[] bytePack = new byte[tabByte.Length];
					if (start != 0) {
						byte[] tmp = new byte[tabByte.Length];
						Array.Copy(tabByte, start, tmp, 0, tabByte.Length - start);
						length = PackDepack.Pack(tmp, tabByte.Length - start, bytePack, 0);
					}
					else
						length = PackDepack.Pack(tabByte, tabByte.Length, bytePack, 0);

					Array.Copy(bytePack, tabByte, length);
					start = 0;
				}
				string name = Path.GetFileNameWithoutExtension(dlg.FileName) + ".asm";
				GenereAsm(tabByte, start, length, 0xA500 - length, startReel, exe, Path.GetDirectoryName(dlg.FileName) + "\\" + name);
				MessageBox.Show("Fichier " + name + " généré.");
			}
		}
	}
}
