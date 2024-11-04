using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1
{/*
    internal class ToBeDeleted
    {

        public Order GetOrder(int id)
        {

            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=*******;Database=NorthWind";
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Sucess\n");
                Order order = null;
                var detailslist = new List<OrderDetails>();

                using (var cmd = new NpgsqlCommand("SELECT orderid, orderdate, requireddate, shipname, shipcity FROM orders where orderid = " + id, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            order = new Order
                            {
                                Id = reader.GetInt32(0),
                                Date = reader.GetDateTime(1),
                                Required = reader.GetDateTime(2),
                                ShipName = reader.GetString(3),
                                ShipCity = reader.GetString(4)

                            };

                            Console.WriteLine($"Order ID: {order.Id}, Date: {order.Date}, Required: {order.Required}, ShipName: {order.ShipName}, ShipCity: {order.ShipCity}");
                        }
                    }
                }

                using (var cmd = new NpgsqlCommand("SELECT productid, unitprice, quantity, discount FROM orderdetails where orderid = " + id, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Product product = new Product();
                            product = GetProduct(reader.GetInt32(0));


                            var orderdetails = new OrderDetails
                            {
                                UnitPrice = reader.GetInt32(1),
                                Quantity = reader.GetInt32(2),
                                Discount = reader.GetInt32(3),
                                Product = product

                            };


                            detailslist.Add(orderdetails);

                        }
                    }
                }
                order.OrderDetails = detailslist;

                return order;
            }

            catch (Exception)
            {

                Console.WriteLine("No Product Found");
            }

            return null;
        }

    }
*/}
