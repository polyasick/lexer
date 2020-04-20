using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace опредение_числа
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        struct Lexem
        {
            public string TokType; 
            public string Value; 

            public Lexem(string type, string value)
            {
                TokType = type;
                Value = value;
            }
        }

        static class LexicalAnalyzer
        {
            public static int transTable(char sym, int state) //Шаблоны
            {
                switch (state)
                {
                    case 0:
                        if (char.IsDigit(sym))
                            return 1;
                        else if (sym == '+' || sym == '*' || sym == '/' || sym == '-' || sym == '^')
                            return 6;
                        else if (char.IsLetter(sym) || sym == '_')
                            return 7;
                        else if (sym == '(')
                            return 8;
                        else if (sym == ')')
                            return 9;
                        else if (sym == ',')
                            return 10;
                        else if (sym == ' ' || sym == '\t' || sym == '\n' || sym == '\r')
                            return 0;
                        break;
                    case 1:
                        if (char.IsDigit(sym))
                            return 1;
                        else if (sym == '.')
                            return 2;
                        else if (sym == 'e' || sym == 'E')
                            return 3;
                        break;
                    case 2:
                        if (char.IsDigit(sym))
                            return 2;
                        else if (sym == 'e' || sym == 'E')
                            return 3;
                        break;
                    case 3:
                        if (sym >= '0' && sym <= '9')
                            return 5;
                        else if (sym == '+' || sym == '-')
                            return 4;
                        break;
                    case 4:
                        if (sym >= '0' && sym <= '9')
                            return 5;
                        break;
                    case 5:
                        if (sym >= '0' && sym <= '9')
                            return 5;
                        break;
                    case 6:
                        break;
                    case 7:
                        if (char.IsLetter(sym) || char.IsDigit(sym) || sym == '_')
                            return 7;
                        break;
                }
                return -1;
            }

            public static string getTokenType(int stId) //Распознавание типа токена
            {
                string tokenType = "Unknown"; 

                if (stId == 0)
                {
                    tokenType = "";
                }
                else if (stId == 1 || stId == 2 || stId == 5) 
                {
                    tokenType = "Number"; 
                }
                else if (stId == 6)
                {
                    tokenType = "Operator";
                }
                else if (stId == 7)
                {
                    tokenType = "Identifier";
                }
                else if (stId == 8)
                {
                    tokenType = "Lparen";
                }
                else if (stId == 9)
                {
                    tokenType = "Rparen";
                }
                else if (stId == 10)
                {
                    tokenType = "Comma";
                }
                return tokenType;
            }

            public static List<Lexem> getLex(string text)
            {
                List<Lexem> lexems = new List<Lexem>(); //Список распознанных лексем

                int st_id = 0;
                string lex1 = ""; //Конкретная лексема в выражении
                int buff = 0;

                for (int i = 0; i < text.Length; i++) //Цикл по символам строки
                {
                    Lexem lex = new Lexem("Unknown", ""); //Пока не начали распознавание очередной лексемы

                    while (i < text.Length && transTable(text[i], st_id) != -1) //Начинаем распознавание очередной лексемы
                    {
                        lex1 += Convert.ToString(text[i]);
                        buff = st_id;
                        st_id = transTable(text[i], buff);
                        i++;
                    }

                    i--;
                    lex.Value = lex1;
                    lex.TokType = getTokenType(transTable(text[i], buff));
                    st_id = 0;
                    lex1 = "";

                    lexems.Add(lex);
                }

                return lexems;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox2.Text;

            var lexems = LexicalAnalyzer.getLex(str); 
            foreach (var lexem in lexems) 
            {
                listBox1.Items.Add(lexem.TokType + " " + lexem.Value);
            }    

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
 

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            textBox2.Clear();
        }
    }
}
