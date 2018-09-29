using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HappyShare.Models;
using HappyShare.Data;

namespace HappyShare.Data
{
    public static class DbInitializer
    {

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            // Look for any students.
            if (context.Categories != null && context.Categories.Any())
            {
                return; // DB has been seeded
            }
            
            // Categories
            var categories = new Category[]
            {
                new Category(){ Name="Furniture", Description="Furnitures, like table, bed, sofa." },
                new Category(){ Name="Appliance", Description="Appliances"},
                new Category(){ Name="Books", Description="Books"},
                new Category(){ Name="Digital devices", Description="Computer, cell phone"},
                new Category(){ Name="Toys", Description="Toys"},
                new Category(){ Name="Pet", Description="PET"},
                new Category(){ Name="Others", Description="Any other things"},
            };
            foreach ( var c in categories )
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            // SharedItems
            
            var items = new SharedItem[] 
            {
                // Furniture
                new SharedItem(){ Name="Table", Description="A used table, almost brand new.", PictureLink="/images/sample/table.jpg", Category=categories[0], Location="{lat: -34.397, lng: 150.644}",Address=" 110 St Heliers, Auckland", ContactorEmail="u1@hs.com", ContactorPhone="022 800 800", Type="Table" },
                new SharedItem(){ Name="Bed", Description="A used bed, king-size.", PictureLink="/images/sample/bed.jpg", Category=categories[0], Location="{lat: -34.397, lng: 150.644}",Address=" 110 St Heliers, Auckland", ContactorEmail="bed@hs.com", ContactorPhone="022 800 810", Type="Bed" },
                new SharedItem(){ Name="Chair", Description="A used table, almost brand new.", PictureLink="/images/sample/chair.jpg", Category=categories[0], Location="{lat: -34.297, lng: 150.644}",Address=" 110 St Lukes, Auckland", ContactorEmail="sofa@hs.com", ContactorPhone="022 800 820", Type="Sofa" },
             
                // Appliance
                new SharedItem(){ Name="TV", Description="A almost new TV(Philips).", PictureLink="/images/sample/TV.jpg", Category=categories[1], Location="{lat: -34.397, lng: 150.614}",Address=" 1 St Heliers, Auckland", ContactorEmail="tv@hs.com", ContactorPhone="021 100 800", Type="TV" },
                new SharedItem(){ Name="iPhone", Description="A brand new one.", PictureLink="/images/sample/iphone.jpg", Category=categories[1], Location="{lat: -34.397, lng: 150.624}",Address=" 2 St Heliers, Auckland", ContactorEmail="iphone@hs.com", ContactorPhone="022 200 810", Type="CellPhone" },
                new SharedItem(){ Name="Washing machine", Description="Need to be reparied.", PictureLink="/images/sample/washingmachine.jpg", Category=categories[1], Location="{lat: -34.297, lng: 150.634}",Address=" 3 St Lukes, Auckland", ContactorEmail="wm@hs.com", ContactorPhone="022 200 820", Type="Washing machine" },
           };
            foreach( var i in items )
            {
                context.SharedItems.Add(i);
            }

            context.SaveChanges();
        }
    }
}