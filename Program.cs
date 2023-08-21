using BankamatikUygulamasi.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankamatikUygulamasi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Context.Context context = new Context.Context();//Veritabanı bağlantısı
            var deger = context.TransactionTypes.ToList();//Banka işlemlerini veritabanından çekiyoruz

            #region Bankamatik_Uygulaması
            string choice, name;
            decimal balance;
            double depositMoney, withdrawMoney, sendMoney;
            decimal witdrawalLimit = 0;
            var customer = Login();//Login işlemi
        Menu:
            name = "";
            choice = "";
            Console.Clear();
            Console.WriteLine($"Aktif Banka Hoşgeldiniz\n{customer.Name} {customer.Surname}\nHangi İşlemi Yapmak İstersiniz");
            Console.WriteLine($"1-{deger[0].TransactionName}\n2-{deger[1].TransactionName}\n3-{deger[2].TransactionName}\n4-Kart İade");
            Console.Write("Seçiniz:");
            choice = Console.ReadLine();

            customer = CustomerRefresh(context, customer);//yapılan işlemlerden sonra güncel bilgiler için
            balance = customer.Balance;
            Console.WriteLine("Lütfen Bekleyiniz...");
            Thread.Sleep(1500);

            if (choice == 1.ToString())
            {
            DepositedMoney:
                Console.Clear();
                depositMoney = 0;
                Console.WriteLine($"Para Yatırma işlemi\nBakiyeniz:{balance} TL");
                Console.Write("Hesabınıza ne kadar para yatırmak istersiniz:");
                CustomerTransactions cT = new CustomerTransactions();
                try
                {

                    depositMoney = Convert.ToDouble(Console.ReadLine());

                    cT.CustomerId = customer.CustomerId;

                    cT.TransactionTypeId = deger[0].TransactionTypeId;
                    cT.AmountOfMoney = (decimal)depositMoney;
                    cT.Explanation = "Para Yatırma";

                    context.CustomerTransactions.Add(cT);
                    var customer2 = context.Customers.Find(customer.CustomerId);
                    customer2.Balance += (decimal)depositMoney;
                    context.SaveChanges();




                    //balance += (decimal)depositMoney;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Bir hata oluştu\nLütfen Bekleyiniz...");
                    Thread.Sleep(1500);
                    goto DepositedMoney;
                }

                Console.WriteLine("Lütfen Bekleyiniz...");
                Thread.Sleep(1000);
                Console.Clear();
                customer = CustomerRefresh(context, customer);
                balance = customer.Balance;
                Console.WriteLine($"{depositMoney} TL hesabınıza yatırıldı." +
                    $"\nGüncel Bakiyeniz:{balance} TL");
                Console.Write("Ana Menüye dönmek için 1'e,\nPara Yatırma işlemine devam etmek için 2'ye,\n" +
                    "Çıkış yapmak için 3'e basınız:");
            Choice:
                choice = Console.ReadLine();
                if (choice == "1")
                    goto Menu;
                else if (choice == "2")
                    goto DepositedMoney;
                else if (choice == "3")
                    Exit(context, customer);
                else
                {
                    Console.Clear();
                    Console.WriteLine("Yanlış bir seçim yaptınız.\nTekrar seçim yapınız");
                    Console.WriteLine("Ana Menü için 1,\nPara Yatırma işlemi için 2,\n" +
                   "Çıkış Yapma İşlemi için 3");
                    Console.Write("Seçiniz:");
                    goto Choice;
                }
            }
            else if (choice == 2.ToString())
            {
            WithdrawMoney:
                withdrawMoney = 0;
                Console.Clear();
                Console.Write($"Para Çekme İşlemi\nBakiyeniz:{balance} TL" +
                    $"\nNe kadar para çekmek istersiniz:");
                try
                {
                    withdrawMoney = Convert.ToDouble(Console.ReadLine());
                    if (withdrawMoney > 5000)
                    {
                        Console.WriteLine("Çekmek istediğiniz miktar günlük limit 5000 TL'nin üstünde." +
                            "\nTekrar Deneyiniz...");
                        Thread.Sleep(1500);
                        goto WithdrawMoney;
                    }
                    else if ((decimal)withdrawMoney > balance)
                    {
                        Console.WriteLine("Bakiyenizde çekmek istediğiniz miktar kadar para yok." +
                            "\nTekrar Deneyiniz...");
                        Thread.Sleep(1500);
                        goto WithdrawMoney;
                    }
                    else
                    {
                        DateTime dateTime = DateTime.Now.AddMilliseconds(-10);
                        if (context.CustomerTransactions.Count(x => x.DateTime > dateTime && x.TransactionTypeId == 2) > 0)
                            witdrawalLimit = (decimal)context.CustomerTransactions.Where(x => x.DateTime > DateTime.Now.Date.AddMilliseconds(-10) && x.TransactionTypeId == 2).Sum(x => x.AmountOfMoney);
                        CustomerTransactions cT = new CustomerTransactions();
                        if (witdrawalLimit < 5001)
                        {
                            cT.CustomerId = customer.CustomerId;

                            cT.TransactionTypeId = deger[1].TransactionTypeId;
                            cT.AmountOfMoney = (decimal)withdrawMoney;
                            cT.Explanation = "Para Çekme";


                            context.CustomerTransactions.Add(cT);
                            var customer2 = context.Customers.Find(customer.CustomerId);
                            customer2.Balance -= (decimal)withdrawMoney;
                            context.SaveChanges();
                            //balance -= (decimal)withdrawMoney;



                        }

                        else
                        {
                            //witdrawalLimit -= withdrawMoney;
                            Console.WriteLine("günlük 5000 TL çekim limitini aştınız." +
                                "\nTekrar Deneyiniz... ");
                            Thread.Sleep(1500);
                            goto WithdrawMoney;
                        }
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Bir hata oluştu.\nLütfen Bekleyiniz...");
                    Thread.Sleep(1500);
                    goto WithdrawMoney;
                }
                Console.WriteLine("Lütfen Bekleyiniz...");
                Thread.Sleep(1500);
                Console.Clear();
                customer = CustomerRefresh(context, customer);
                balance = customer.Balance;
                Console.WriteLine($"Lütfen {withdrawMoney} TL'nizi hazneden alınız." +
                    $"\nGüncel Bakiyeniz:{balance}");
                Console.Write("Menüye dönmek için 1'e" +
                    "\nPara çekme işlemine devam etmek için 2'ye\nÇıkış yapmak için 3'e basınız:");
            Choice:
                choice = Console.ReadLine();
                if (choice == "1")
                    goto Menu;
                else if (choice == "2")
                    goto WithdrawMoney;
                else if (choice == "3")
                    Exit(context, customer);
                else
                {
                    Console.Clear();
                    Console.WriteLine("Bir hata oluştu.\nTekrar seçim yapınız...");
                    Thread.Sleep(1500);
                    Console.Write("Menüye dönmek için 1'e" +
                   "\nPara çekme işlemine devam etmek için 2'ye\nÇıkış yapmak için 3'e basınız:");

                    goto Choice;
                }


            }
            else if (choice == 3.ToString())
            {
            SendMoney:
                sendMoney = 0;
                Console.Clear();
                Console.Write($"Para Gönderme İşlemi\nBakiyeniz:{balance}" +
                    $"\nNe kadar para göndermek istersiniz:");
                try
                {
                    sendMoney = Convert.ToDouble(Console.ReadLine());
                    if ((decimal)sendMoney > balance)
                    {
                        Console.WriteLine("Bakiyenizde para göndermek için yeterli miktarda para yok." +
                            "\nLütfen Tekrar Deneyiniz...");
                        Thread.Sleep(1500);
                        goto SendMoney;
                    }
                    else
                    {

                        //balance -= (decimal)sendMoney;
                        Console.Write("Para göndermek istediğiniz kişi\\kurum adını yazınız:");
                        name = Console.ReadLine();
                        CustomerTransactions cT = new CustomerTransactions();
                        cT.CustomerId = customer.CustomerId;
                        cT.Explanation = name;
                        cT.AmountOfMoney = (decimal)sendMoney;
                        cT.TransactionTypeId = 2;

                        var customer2 = context.Customers.Find(customer.CustomerId);
                        customer2.Balance -= (decimal)sendMoney;
                        context.CustomerTransactions.Add(cT);
                        context.SaveChanges();
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Bir hata oluştu.\nLütfen bekleyiniz...");
                    Thread.Sleep(1500);
                    goto SendMoney;
                }
                Console.WriteLine("Lütfen Bekleyiniz...");
                Thread.Sleep(1500);
                Console.Clear();
                customer = CustomerRefresh(context, customer);
                balance = customer.Balance;
                Console.WriteLine($"{sendMoney} TL {name} alıcısına gönderildi." +
                    $"\nGüncel Bakiyeniz:{balance}");
                Console.Write("Menüye dönmek için 1'e\nPara gönderme işlemine devam etmek için 2'ye" +
                    "\nÇıkış yapmak için 3'e basınız:");
            Choice:
                choice = Console.ReadLine();
                if (choice == "1")
                    goto Menu;
                else if (choice == "2")
                    goto SendMoney;
                else if (choice == "3")
                {
                    Exit(context, customer);
                }

                else
                {
                    Console.Clear();
                    Console.WriteLine("Bir hata oluştu.\nTekrar Seçim Yapınız");
                    Thread.Sleep(1500);
                    Console.Write("Menüye dönmek için 1'e\nPara gönderme işlemine devam etmek için2'ye" +
                  "\nÇıkış yapmak için 3'e basınız:");
                    goto Choice;
                }

            }
            else if (choice == 4.ToString())
            {
                Console.Clear();
                Console.WriteLine("İşleminiz yapılıyor lütfen bekleyiniz...");
                Thread.Sleep(1000);
                Exit(context, customer);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Seçim yapılırken bir hata oluştu tekrar Ana Menüye yönlendiriliyorsunuz...");
                Thread.Sleep(1500);
                goto Menu;

            }


            #endregion
        }

        private static void Exit(Context.Context context, Customer customer)
        {
            CustomerLogs cL = new CustomerLogs();
            cL.CustomerId = customer.CustomerId;
            cL.IsLoginOrLogout = false;
            context.CustomerLogs.Add(cL);
            context.SaveChanges();

            Environment.Exit(0);
        }

        private static Customer CustomerRefresh(Context.Context context, Customer customer)
        {
            customer = context.Customers.Find(customer.CustomerId);
            return customer;
        }

        private static Customer Login()
        {
            Context.Context context = new Context.Context();

        Login:
            Console.WriteLine("Merhaba Hoş Geldiniz...");
            Console.Write("Kullanici Adi:");
            string username = Console.ReadLine();
            Console.Write("Şifre:");
            string password = Console.ReadLine();

            var deger = context.Customers.FirstOrDefault(x => x.Name == username && x.Password == password);
            if (deger is null)
            {
                Console.WriteLine("Kullanici adi veya şifreniz yanlış.");
                Thread.Sleep(1000);
                Console.Clear();
                goto Login;

            }

            else
            {
                CustomerLogs customerLogs = new CustomerLogs();
                customerLogs.CustomerId = deger.CustomerId;
                customerLogs.Customer = deger;
                customerLogs.IsLoginOrLogout = true;
                context.CustomerLogs.Add(customerLogs);
                if (context.SaveChanges() > 0)
                {
                    return deger;
                }

                Console.WriteLine("Bir Hata oluştu.");
                Thread.Sleep(1000);
                Console.Clear();
                goto Login;

            }

        }
    }
}
