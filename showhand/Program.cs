using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace showhand
{
    class Program
    {
        static Random RD = new Random();
        static string[] card_color_name = { "梅花", "方塊", "愛心", "黑桃" };
        static string[] card_number_name = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        static string[] card_type = { "高牌", "對子", "兩對", "三條", "順子", "同花", "葫蘆", "四條", "同花順" };
        static string[] WinLose = { "P1Win", "P2Win" };
        //p1籌碼
        static int p1chips = 1000;
        //p2籌碼
        static int p2chips = 1000;
        //有幾張明牌
        static int count = 1;
        //給棄牌堆用的i
        static int used_i = 0;
        //棄牌堆
        static int[] used_card = new int[52];
        //p1手牌
        static int[] handcard = new int[5];
        //p2手牌
        static int[] handcard2 = new int[5];
        //測試牌
        //static int[] testcard = { 21, 22, 21, 22 };
        //測試牌2
        //static int[] testcard2 = { 13, 24, 33, 43, 134 };
        static void Main(string[] args)
        {
            //底注
            int pot = 100;
            p1chips -= 50;
            p2chips -= 50;
            handcard[0] = Give_a_card();//p1發一張蓋牌
            handcard2[0] = Give_a_card();//p2發一張蓋牌
            int bet = 0;
            for (int i = 1; i < 5; i++)
            {
                handcard[i] = Give_a_card();//p1發第i張明牌
                handcard2[i] = Give_a_card();//p2發第i張明牌
                Console.WriteLine("P1：\n");
                Console.WriteLine("蓋牌：{0}\n", PrintCard(handcard, 0));
                Console.Write("明牌：");
                for (int j = 1; j <= i; j++)
                {
                    Console.Write(PrintCard(handcard, j) + " ");
                }
                Console.WriteLine("\n\n籌碼：{0}", p1chips);
                Console.WriteLine("\nP2：\n");
                Console.WriteLine("蓋牌：{0}\n", PrintCard(handcard2, 0));
                Console.Write("明牌：");
                for (int j = 1; j <= i; j++)
                {
                    Console.Write(PrintCard(handcard2, j) + " ");
                }
                Console.WriteLine("\n\n籌碼：{0}", p2chips);
                Console.WriteLine("\n底池：{0}\n", pot);
                if (Win_or_Lose(handcard, handcard2, count) == 0)//P1明牌較大
                {
                    Console.WriteLine("P1牌大，輸入加注金額，若不加注輸入0：");
                    bet = Convert.ToInt32(Console.ReadLine());
                    p1chips -= bet;
                    pot += bet;
                    if (P1Bet(ref bet, ref pot) == 1)
                    {
                        break;
                    }

                    Console.WriteLine();
                }
                else//P2明牌較大
                {
                    Console.WriteLine("P2牌大，輸入加注金額，若不加注輸入0：");
                    bet = Convert.ToInt32(Console.ReadLine());
                    p2chips -= bet;
                    pot += bet;
                    if (P2Bet(ref bet, ref pot) == 1)
                    {
                        break;
                    }

                    Console.WriteLine();
                }

                count++;
            }

            if (count == 5)
            {//照大小顯示5張牌
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(PrintCard(Sort(handcard, 5), i));
                }
                //顯示 A對子 5三條 7葫蘆...等等
                if (Type(handcard, 5)[0] == 1 || Type(handcard, 5)[0] == 3 || Type(handcard, 5)[0] == 6 || Type(handcard, 5)[0] == 7)
                {
                    Console.Write(card_number_name[Type(handcard, 5)[1] - 1]);
                }
                //針對兩對顯示 58兩對...等等
                if (Type(handcard, 5)[0] == 2)
                {
                    Console.Write(card_number_name[Type(handcard, 5)[1] - 1] + " " + card_number_name[Type(handcard, 5)[2] - 1]);
                }
                //p1牌型
                Console.WriteLine(card_type[Type(handcard, 5)[0]]);
                Console.WriteLine();
                //照大小顯示5張牌
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(PrintCard(Sort(handcard2, 5), i));
                }
                //顯示 A對子 5三條 7葫蘆...等等
                if (Type(handcard2, 5)[0] == 1 || Type(handcard2, 5)[0] == 3 || Type(handcard2, 5)[0] == 6 || Type(handcard2, 5)[0] == 7)
                {
                    Console.Write(card_number_name[Type(handcard2, 5)[1] - 1]);
                }
                //針對兩對顯示 58兩對...等等
                if (Type(handcard2, 4)[0] == 2)
                {
                    Console.Write(card_number_name[Type(handcard2, 5)[1] - 1] + " " + card_number_name[Type(handcard2, 5)[2] - 1]);
                }
                //p2牌型
                Console.WriteLine(card_type[Type(handcard2, 5)[0]]);
                if (Win_or_Lose(handcard, handcard2, count) == 0)
                {
                    p1chips += pot;
                    Console.WriteLine("P1Win");
                    Console.WriteLine("P1籌碼：{0}", p1chips);
                    Console.WriteLine("P2籌碼：{0}", p2chips);
                }
                else if (Win_or_Lose(handcard, handcard2, count) == 1)
                {
                    p2chips += pot;
                    Console.WriteLine("P2Win");
                    Console.WriteLine("P1籌碼：{0}", p1chips);
                    Console.WriteLine("P2籌碼：{0}", p2chips);
                }
            }
            pot = 0;
            used_i = 0;

            Console.Read();
        }

        static int P1Bet(ref int bet, ref int pot)
        {
            int bbet = bet;//bet為P1下注金額，先存在bbet裡
            if (bet != 0)
            {
                Console.WriteLine("P1籌碼：{0}\n底池：{1}", p1chips, pot);
                Console.WriteLine("P1加注{0}，P2是否跟注加注或棄牌，棄牌輸入-1跟注輸入0.加注輸入金額：", bet);
            }
            else
            {
                Console.WriteLine("P1過牌，P2是否過牌或加注，過牌輸入0.加注輸入金額：");
            }
            bet = Convert.ToInt32(Console.ReadLine());//P2寫入下注金額
            if (bet > 0)
            {
                p2chips -= (bet + bbet);
                pot += (bet + bbet);
                P2Bet(ref bet, ref pot);
            }
            else if (bet == 0)
            {
                p2chips -= bbet;
                pot += bbet;
                Console.WriteLine("P2籌碼：{0}", p2chips);
                Console.WriteLine("底池：{0}", pot);
            }
            else//棄牌時
            {
                Console.WriteLine("P2棄牌，P1贏得底池");
                p1chips += pot;
                Console.WriteLine("P1籌碼{0}", p1chips);
                Console.WriteLine("P2籌碼{0}", p2chips);
                return 1;
            }
            return 0;
        }
        static int P2Bet(ref int bet, ref int pot)
        {
            int bbet = bet;
            if (bet != 0)
            {
                Console.WriteLine("P2籌碼：{0}\n底池：{1}", p2chips, pot);
                Console.WriteLine("P2加注{0}，P1是否跟注加注或棄牌，棄牌輸入-1跟注輸入0.加注輸入金額：", bet);
            }
            else
            {
                Console.WriteLine("P2過牌，P1是否過牌或加注，過牌輸入0.加注輸入金額：");
            }
            bet = Convert.ToInt32(Console.ReadLine());
            if (bet > 0)
            {
                p1chips -= (bet + bbet);
                pot += (bet + bbet);
                P1Bet(ref bet, ref pot);
            }
            else if (bet == 0)
            {
                p1chips -= bbet;
                pot += bbet;
                Console.WriteLine("P1籌碼：{0}", p1chips);
                Console.WriteLine("底池：{0}", pot);
            }
            else//棄牌時
            {
                Console.WriteLine("P1棄牌，P2贏得底池");
                p2chips += pot;
                Console.WriteLine("P1籌碼{0}", p1chips);
                Console.WriteLine("P2籌碼{0}", p2chips);
                return 1;
            }
            return 0;
        }
        //發一張牌並判斷牌是否有發過
        static int Give_a_card()
        {
            int card = 0;
            //used表示牌有無發過，used為true，執行此迴圈
            for (bool used = true; used == true;)
            {
                used = false;
                //發一張牌，令數字為百位數和十位數，花色為個位數
                card = RD.Next(1, 14) * 10 + RD.Next(1, 5);
                //判斷發的牌和棄牌堆有沒有重複
                for (int j = 0; j < 52; j++)
                {
                    //若有重複，令used為true，中斷此迴圈
                    if (used_card[j] == card)
                    {
                        used = true;
                        break;
                    }
                }
            }
            //都沒重複時，將此牌新增到用過的牌堆裡
            used_card[used_i] = card;
            used_i++;
            return card;
        }

        //回傳花色和數字的字串
        //百位和十位數1.2.3....11.12.13分別是2.3.4....Q.K.A
        //個位數1=梅花.2=方塊.3=愛心.4=黑桃
        static string PrintCard(int[] card, int choose)
        {
            return card_color_name[card[choose] % 10 - 1] + " " + card_number_name[card[choose] / 10 - 1];
        }

        //手牌大小排序(冒泡排序法)
        static int[] Sort(int[] handcard, int count)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (handcard[i] > handcard[j])
                    {
                        int a = handcard[i];
                        handcard[i] = handcard[j];
                        handcard[j] = a;
                    }
                }
            }
            return handcard;
        }

        //手牌牌型
        static int[] Type(int[] type_handcard, int count)
        {
            int[] handcard = new int[count + 1];//新增一個陣列
            //將手牌複製到此陣列
            if (count == 5)
            {
                for (int i = 0; i < count; i++)
                {
                    handcard[i] = type_handcard[i];
                }
            }
            else
            {
                for (int i = 0; i < count + 1; i++)
                {
                    handcard[i] = type_handcard[i];
                }
            }
            //當明牌小於5時，第一張牌還是蓋牌，不比第一張牌，將陣列中每一項往前移一位
            if (count < 5)
            {
                for (int i = 0; i < count; i++)
                {
                    handcard[i] = handcard[i + 1];
                    if (i == count - 1)
                    {
                        handcard[i + 1] = 0;
                    }
                }

            }
            //排序此手牌
            handcard = Sort(handcard, count);
            int[] type = new int[7];
            //5張手牌才需要判斷順或同花
            if (count == 5)
            {
                //判斷是否為順
                bool straight = true;
                //若第i張牌的數字+1不等於第i+1張牌的數字，布林值為false
                for (int i = 0; i < 4; i++)
                {
                    if (handcard[i] / 10 + 1 != handcard[i + 1] / 10)
                    {
                        //若是A2345時，判斷為順子
                        if (i == 3 && handcard[3] / 10 == 4 && handcard[4] / 10 == 13)
                        {
                            break;
                        }
                        straight = false;
                        break;
                    }
                }

                //判斷是否為同花
                bool flush = true;
                for (int i = 0; i < 4; i++)
                {
                    if (handcard[i] % 10 != handcard[i + 1] % 10)
                    {
                        flush = false;
                        break;
                    }
                }
                //同花順
                if (flush == true && straight == true)
                {
                    type[0] = 8;
                    type[1] = handcard[4] / 10;
                    type[2] = handcard[3] / 10;//2345A為第二大順，最大順為10JQKA
                    type[3] = handcard[4] % 10;//花色
                    return type;
                }
                //同花
                else if (flush == true)
                {
                    type[0] = 5;
                    for (int i = 1, j = 4; i < 6; i++, j--)
                    {
                        type[i] = handcard[j] / 10;
                    }
                    type[6] = handcard[4] % 10;//最大數字的花色
                    return type;
                }
                //順子
                else if (straight == true)
                {
                    type[0] = 4;
                    type[1] = handcard[4] / 10;
                    type[2] = handcard[3] / 10;//2345A為第二大順，最大順為10JQKA
                    type[3] = handcard[4] % 10;//最大數字的花色
                    return type;
                }
            }
            int max = 0;
            for (int i = 0; i < count - 1; i++)
            {
                if (handcard[i] / 10 == handcard[i + 1] / 10)
                {
                    max++;
                }
            }

            for (int i = 0; i < count - 1; i++)
            {
                //四條
                if (max == 3 && i < 2 && handcard[i] / 10 == handcard[i + 3] / 10)
                {
                    type[0] = 7;
                    type[1] = handcard[i] / 10;
                    return type;
                }
                //三條
                else if (max == 2 && i < 3 && handcard[i] / 10 == handcard[i + 2] / 10)
                {
                    type[0] = 3;
                    type[1] = handcard[i] / 10;
                    return type;
                }
                //一對
                else if (max == 1 && handcard[i] / 10 == handcard[i + 1] / 10)
                {
                    type[0] = 1;
                    type[1] = handcard[i] / 10;
                    type[6] = handcard[i + 1] % 10;//對子最大的花色
                    //除對子以外的牌，由大到小放進type[]
                    for (int j = 2, k = count - 1; j < count; k--)
                    {
                        if (k != i && k != i + 1)
                        {
                            type[j] = handcard[k] / 10;
                            j++;
                        }
                    }
                    return type;
                }
            }
            //葫蘆
            if (max == 3)
            {
                type[0] = 6;
                type[1] = handcard[2] / 10;
                return type;
            }
            //兩對
            else if (max == 2)
            {
                type[0] = 2;
                for (int i = 0; i < count - 3; i++)
                {
                    for (int j = 2; j < count - 1; j++)
                    {
                        if (handcard[i] / 10 == handcard[i + 1] / 10 && handcard[j] / 10 == handcard[j + 1] / 10)
                        {
                            type[1] = handcard[j] / 10;//兩對中大的對子
                            type[4] = handcard[j + 1] % 10;//大的對子中大的花色
                            type[2] = handcard[i] / 10;//兩對中小的對子
                            //對子外的那張牌，放入type[]
                            for (int k = 0; k < count; k++)
                            {
                                if (k != i && k != i + 1 && k != j && k != j + 1)
                                {
                                    type[3] = handcard[k] / 10;
                                }
                            }
                        }
                    }
                }
                return type;
            }
            //高牌
            type[0] = 0;
            type[6] = handcard[count - 1] % 10;
            for (int i = 1, j = count - 1; i < count + 1; i++, j--)
            {
                type[i] = handcard[j] / 10;
            }
            return type;
        }

        //判斷輸贏
        static int Win_or_Lose(int[] p1, int[] p2, int count)
        {
            int[] p1type = Type(p1, count);
            int[] p2type = Type(p2, count);

            for (int i = 0; i < 7; i++)
            {
                if (p1type[i] > p2type[i])
                {
                    return 0;
                }
                else if (p2type[i] > p1type[i])
                {
                    return 1;
                }
            }
            return -1;
        }
    }
}
