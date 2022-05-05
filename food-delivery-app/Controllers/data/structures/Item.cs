﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace food_delivery_app.Controllers.data.structures
{
    public class Item
    {
        public int id;
        public string name;
        public string description;
        public double price;

        public List<Selection> selections;

        public async Task QuerySelections(int id)
        {
            this.id = id;
            selections = new List<Selection>();
            string query = @"select selections.id,selections.name,selections.optional,selections.price 
from items join selections on items.id = selections.item_id where items.id = ($1);";

            await using var cmd = new NpgsqlCommand(query, Database.connection)
            {
                Parameters =
                {
                    new() {Value = id}
                }
            };
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Selection selection = new Selection()
                {
                    id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    optional = reader.GetBoolean(2),
                    price = reader.GetDouble(3),
                };
                selections.Add(selection);
            }
        }

    }

    public class Selection
    {
        public int id;
        public string name;
        public bool optional = false;
        public double price;
    }
}
