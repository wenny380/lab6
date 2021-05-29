using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SERVER
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    class Seats
    { //Свободные места
        public int num;
        public bool free;


        public Seats(int number, bool tr)
        {

            this.num = number;
            this.free = tr;

        }
    }
    class Route //Маршруты
    {
        public string route_id;
        public string from;
        public string to;
        public string data;
        public int count;
        public Seats[] se =
            {
                new Seats (1 , true),
                new Seats (2 , true),
                new Seats (3 , true),
                new Seats (4 , true),
                new Seats (5 , true),
                new Seats (6 , true),
                new Seats (7 , true),
                new Seats (8 , true),
                new Seats (9 , true),
                new Seats (10 , true),
            };
        public Route(string route_id, string from, string to, string data, int count)
        {
            this.route_id = route_id;
            this.from = from;
            this.to = to;
            this.data = data;
            this.count = count;

        }

    }

    public class WebService1 : WebService
    {
        static int[] counts = new int[7];
        static int[,] frees = new int[7,11];
        string response = "";
        string responseSeats = "";
        Route[] routes = {  
            new Route("1", "Port-au-Prince", "Martissant", "24/10/2021 22:22", 10),
            new Route("2", "Baraderes", "Miami", "20/10/2021 22:22", 10),
            new Route("3", "Camp-Perrin", "New-York", "20/10/2021 22:22", 10),
            new Route("4", "Pernier", "Washighton",  "20/10/2021 22:22", 10),
            new Route("5", "Cap-Haitien", "Jeremie", "21/10/2021 22:22", 10),
            new Route("6", "Jeremie", "Jacmel",  "23/10/2021 22:22", 10),
        };

        [WebMethod]
        public string ShowRoutes() //Show all the flight
        {
            response = "";

            for (int i = 0; i < routes.Length; i++)
            {
                response = response + routes[i].route_id + " " + routes[i].from + "---" + routes[i].to + "\r\n";
            }

            return response;
        }

        [WebMethod]
        public string CheckRoutes(string Route_id) //Check the departure's time
        {
            response = "";
            for (int i = 0; i < routes.Length; i++)
            {

                if (Route_id == routes[i].route_id)
                {
                    int cou = routes[i].count - counts[i];
                    routes[i].count = cou;
                    response = response + routes[i].route_id + "   " + routes[i].from + "---" + routes[i].to + "   " + routes[i].data + "  Осталось билетов:" + routes[i].count + "\r\n";
                }
            }

            return response;
        }
        [WebMethod]
        public string ShowSeats(string Route_id) //Show the seats left
        {
            response = "";
            for (int i = 0; i < routes.Length; i++)
            {
                if (Route_id == routes[i].route_id)
                {
                    response = response + routes[i].route_id + "   " + routes[i].from + "---" + routes[i].to + "   " + routes[i].data + "  Остались места:" + " ";
                    for (int j = 0; j < routes[i].count; j++)
                    {
                        if (frees[i, j] == 0)
                        {
                            responseSeats = responseSeats + " " + routes[i].se[j].num;
                        }
                    }
                    response = response + responseSeats + "\r\n";

                }
            }
            return response;
        }


        [WebMethod]
        public string buytickets(string Route_id, int num) //Book a ticket
        {
            response = "";
            for (int i = 0; i < routes.Length; i++)
            {

                if ((Route_id == routes[i].route_id) && (routes[i].count != 0) && (routes[i].count >= 1))
                {     
                    for (int j = 0; j < routes[i].count; j++)
                    {
                        if ((routes[i].se[j].num == num) && (frees[i, j] == 0))
                        {
                            routes[i].se[j].free = false;
                            counts[i] = counts[i] + 1;
                            frees[i,j] = 1;
                            routes[i].count = routes[i].count - 1;
                            response = "Ваш заказ выполнен";
                            return response;
                        }
                    }
                }
                else response = "Ваш заказ не выполнен\r\n Проверьте следующие данные:\r\n\n -Номер маршрута\r\n -Выбранное место должно быть свободно\r\n -Наличие билетов";
            }
            return response;
        }
    }
}