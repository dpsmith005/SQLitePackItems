using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace SQLitePackItems
{
    public interface IPackItemsRepository
    {
        IEnumerable<PackItemDGV> GetGridItems(string kind, string value);
        IEnumerable<PackItem> GetAllPackItems();
        IEnumerable<PackItem> GetSinglePackItem(string value);
        IEnumerable<GroupItem> GetAllGroups();
        IEnumerable<TagItem> GetAllTags();
        void PackItemDelete(string item);
        void PackItemInsert(PackItem packItem);
        void PackItemUpdate(PackItem packItem);
        void GroupItemUpdate(string GroupName, int ListOrder);
        IEnumerable<PackItemsLO> GetAllPackItemsLO();
        IEnumerable<PackItemsLO> GetAllPackItemsLOQuery(string query);
    }

    public class PackItemsRepository : IPackItemsRepository
    {
        private static string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static string connectionString = $@"Data Source={path}\Files\SQLitePack.db;Version=3";
        //private static string connectionString = @"Data Source=..\..\..\Files\SQLitePack.db;Version=3";

        public IEnumerable<PackItemDGV> GetGridItems(string kind, string value)
        {
            List<PackItemDGV> PackItemDGV = new List<PackItemDGV>();
            string query;

            switch (kind)
            {
                case "Item":
                    query = "select pit.Item, pit.GroupName, pit.Tags, pit.Id from PackItems AS pit INNER JOIN Groups AS gr on gr.GroupName = pit.GroupName WHERE pit.Item like '%" + value + "%' ORDER BY gr.ListOrder, pit.Item";
                    break;
                case "Group":
                    query = "select pit.Item, pit.GroupName, pit.Tags, pit.Id from PackItems AS pit INNER JOIN Groups AS gr on gr.GroupName = pit.GroupName WHERE pit.GroupName like '" + value + "%' ORDER BY gr.ListOrder, pit.Item";
                    break;
                case "Tag":
                    query = "select pit.Item, pit.GroupName, pit.Tags, pit.Id from PackItems AS pit INNER JOIN Groups AS gr on gr.GroupName = pit.GroupName WHERE pit.Tags like '" + value + "%' ORDER BY gr.ListOrder, pit.Item";
                    break;
                default:
                    query = "select pit.Item, pit.GroupName, pit.Tags, pit.Id from PackItems AS pit INNER JOIN Groups AS gr on gr.GroupName = pit.GroupName ORDER BY gr.ListOrder, pit.Item";
                    break;
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("Id"));
                            string item = reader.GetString(reader.GetOrdinal("Item"));
                            string groupName = reader.GetString(reader.GetOrdinal("GroupName"));
                            string tags = reader.GetString(reader.GetOrdinal("Tags"));
                            PackItemDGV.Add(new PackItemDGV(item, groupName, tags, id));
                        }
                    }
                }
            }
            return PackItemDGV; 
        }
        public IEnumerable<PackItem> GetAllPackItems() 
        {
            List<PackItem> PackItem = new List<PackItem>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT pit.* FROM PackItems AS pit INNER JOIN Groups AS gr on gr.GroupName = pit.GroupName  ORDER BY gr.ListOrder, pit.Item";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("Id"));
                            string item = reader.GetString(reader.GetOrdinal("Item"));
                            string groupName = reader.GetString(reader.GetOrdinal("GroupName"));
                            string tags = reader.GetString(reader.GetOrdinal("Tags"));
                            string notes = reader.GetString(reader.GetOrdinal("Notes"));
                            int grams = reader.GetInt32(reader.GetOrdinal("grams"));
                            decimal ounces = reader.GetDecimal(reader.GetOrdinal("ounces"));    //calculated: grams * 28.3495 (This i grams per ounce.  0.035274 is oz/gm)
                            int lb = reader.GetInt32(reader.GetOrdinal("lb"));                //calculated: ounces % 16
                            decimal oz = reader.GetDecimal(reader.GetOrdinal("oz"));            //calculated: ounces - ((ounces % 16) * 16)
                            bool _new = reader.GetBoolean(reader.GetOrdinal("New"));
                            bool selected = reader.GetBoolean(reader.GetOrdinal("Selected")); ;
                            PackItem.Add(new PackItem(id, groupName, item, grams, ounces, lb, oz, _new, selected, tags, notes));
                        }
                    }
                }
            }

            return PackItem;
        }
        public IEnumerable<PackItem> GetSinglePackItem(string value)
        {
            List<PackItem> PackItem = new List<PackItem>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM PackItems AS pit INNER JOIN Groups AS gr on gr.GroupName = pit.GroupName WHERE pit.Id == '" + value + "'  ORDER BY gr.ListOrder, pit.Item";
                // AS pit INNER JOIN Groups AS gr on gr.GroupName = pit.GroupName  ORDER BY gr.ListOrder

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("Id"));
                            string item = reader.GetString(reader.GetOrdinal("Item"));
                            string groupName = reader.GetString(reader.GetOrdinal("GroupName"));
                            string tags = reader.GetString(reader.GetOrdinal("Tags"));
                            string notes = reader.GetString(reader.GetOrdinal("Notes"));
                            int grams = reader.GetInt32(reader.GetOrdinal("grams"));
                            decimal ounces = reader.GetDecimal(reader.GetOrdinal("ounces"));    //calculated: grams * 28.3495 (This i grams per ounce.  0.035274 is oz/gm)
                            int lb = reader.GetInt32(reader.GetOrdinal("lb"));                //calculated: ounces % 16
                            decimal oz = reader.GetDecimal(reader.GetOrdinal("oz"));            //calculated: ounces - ((ounces % 16) * 16)
                            bool _new = reader.GetBoolean(reader.GetOrdinal("New"));
                            bool selected = reader.GetBoolean(reader.GetOrdinal("Selected")); ;
                            PackItem.Add(new PackItem(id, groupName, item, grams, ounces, lb, oz, _new, selected, tags, notes));
                        }
                    }
                }
            }

            return PackItem;
        }
        public IEnumerable<GroupItem> GetAllGroups()
        {
            List<GroupItem> grpItems = new List<GroupItem>();
            //grpItems.Add(new GroupItem("", 0));   // Add blank group item
            using (SQLiteConnection connection =new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "select * from Groups order by ListOrder,GroupName;";             //string query = "select Distinct GroupName from PackItems where GroupName<> '' ORDER BY GroupName;";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        { 
                            string groupName = reader.GetString(reader.GetOrdinal("GroupName"));
                            int listOrder = reader.GetInt32(reader.GetOrdinal("ListOrder"));
                            grpItems.Add(new GroupItem(groupName,listOrder));
                        }
                    }
                }
            }
            return grpItems;
        }
        public IEnumerable<TagItem> GetAllTags() 
        {
            List<TagItem> tagItems = new List<TagItem>();
            //tagItems.Add(new TagItem(""));  // Add blank tag item
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "select * from Tags order by TagName;";             //string query = "select Distinct GroupName from PackItems where GroupName<> '' ORDER BY GroupName;";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tagName = reader.GetString(reader.GetOrdinal("TagName"));
                            tagItems.Add(new TagItem(tagName));
                        }
                    }
                }
            }
            return tagItems;
        }
        public void PackItemDelete(string item) 
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM[PackItems] WHERE Item = '" + item + "'";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    MainForm._Form1.updateStatus("About to delete this item - " + item);
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete Data", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        command.ExecuteNonQuery();
                        MainForm._Form1.updateStatus("");  //Clear Status Windows
                        MainForm._Form1.updateStatus("Deleted item from table - " + item );
                    } else
                    {
                        MainForm._Form1.updateStatus("Item delete canceled");
                    }
                }
            }
        }
        public void PackItemInsert(PackItem packItem) 
        {
            MainForm._Form1.updateStatus("Inserting item into table");
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string values = "'"+packItem.GroupName+"','"+packItem.Item+"',"+packItem.grams+","+packItem.ounces+","+packItem.lb+","+packItem.oz+",'"+packItem.Notes+"',"+packItem.New+","+packItem.Selected+",'"+packItem.Tags+"'";
                string query = "INSERT INTO [PackItems] ([GroupName], [Item], [grams], [ounces], [lb], [oz], [Notes], [New], [Selected], [Tags]) VALUES ("+values+");";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    try
                    {
                        MainForm._Form1.updateStatus("About to insert this item - " + packItem.Item);
                        command.ExecuteNonQuery();
                        MainForm._Form1.updateStatus("");  //Clear Status Windows
                        MainForm._Form1.updateStatus("Inserted item to table - " + packItem.Item);
                    }
                    catch
                    {
                        MainForm._Form1.updateStatus("Failed to insert item");
                    }
                }
            }
        }
        public void PackItemUpdate(PackItem packItem) 
        {
            MainForm._Form1.updateStatus("Updating item in table");
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $@"UPDATE [PackItems] 
                       SET [GroupName] = '{packItem.GroupName}'
                          ,[Item] = '{packItem.Item}'
                          ,[grams] = {packItem.grams}
                          ,[ounces] = {packItem.ounces}
                          ,[lb] = {packItem.lb}
                          ,[oz] = {packItem.oz}
                          ,[Notes] = '{packItem.Notes}'
                          ,[New] = {packItem.New}
                          ,[Selected] = {packItem.Selected}
                          ,[Tags] = '{packItem.Tags}'
                     WHERE id = {packItem.Id};
                 ";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                        try
                        {
                            MainForm._Form1.updateStatus("About to update this item - " + packItem.Item);
                            command.ExecuteNonQuery();
                            MainForm._Form1.updateStatus("");  //Clear Status Windows
                            MainForm._Form1.updateStatus("Updated item in table - " + packItem.Item);
                        }
                        catch
                        {
                            MainForm._Form1.updateStatus("Failed to update item");
                        }
                }
            }
        }
        public void GroupItemUpdate(string GroupName, int ListOrder)
        {
            MainForm._Form1.updateStatus("Updating Groups order number.  Item: " + GroupName + " Order: " + ListOrder );
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $@"UPDATE [Groups] 
                    SET [ListOrder] = {ListOrder}
                    WHERE [GroupName] = '{GroupName}'
            ";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    try
                    {
                        MainForm._Form1.updateStatus("Updating Groups order number.  Item: " + GroupName + " Order: " + ListOrder);
                        command.ExecuteNonQuery();
                        //MainForm._Form1.updateStatus("");  //Clear Status Windows
                        MainForm._Form1.updateStatus("Updated item in Groups table.  Item: " + GroupName + " Order: " + ListOrder);
                    }
                    catch
                    {
                        MainForm._Form1.updateStatus("Failed to update Group item for " + GroupName);
                    }
                }
            }
        }
        public IEnumerable<PackItemsLO> GetAllPackItemsLO()
        {
            List<PackItemsLO> PackItemsLO = new List<PackItemsLO>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PackItems.*, gr.ListOrder FROM PackItems INNER JOIN Groups AS gr on gr.GroupName = PackItems.GroupName  ORDER BY gr.ListOrder, PackItems.Item";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("Id"));
                            string item = reader.GetString(reader.GetOrdinal("Item"));
                            string groupName = reader.GetString(reader.GetOrdinal("GroupName"));
                            string tags = reader.GetString(reader.GetOrdinal("Tags"));
                            string notes = reader.GetString(reader.GetOrdinal("Notes"));
                            int grams = reader.GetInt32(reader.GetOrdinal("grams"));
                            decimal ounces = reader.GetDecimal(reader.GetOrdinal("ounces"));    //calculated: grams * 28.3495 (This i grams per ounce.  0.035274 is oz/gm)
                            int lb = reader.GetInt32(reader.GetOrdinal("lb"));                //calculated: ounces % 16
                            decimal oz = reader.GetDecimal(reader.GetOrdinal("oz"));            //calculated: ounces - ((ounces % 16) * 16)
                            bool _new = reader.GetBoolean(reader.GetOrdinal("New"));
                            bool selected = reader.GetBoolean(reader.GetOrdinal("Selected"));
                            int ListOrder = reader.GetInt32(reader.GetOrdinal("ListOrder"));
                            PackItemsLO.Add(new PackItemsLO(id, groupName, item, grams, ounces, lb, oz, _new, selected, tags, notes, ListOrder));
                        }
                    }
                }
            }

            return PackItemsLO;
        }
        public IEnumerable<PackItemsLO> GetAllPackItemsLOQuery(string query)
        {
            List<PackItemsLO> PackItemsLO = new List<PackItemsLO>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                //string query = "SELECT PackItems.*, gr.ListOrder FROM PackItems INNER JOIN Groups AS gr on gr.GroupName = PackItems.GroupName  ORDER BY gr.ListOrder, PackItems.Item";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("Id"));
                            string item = reader.GetString(reader.GetOrdinal("Item"));
                            string groupName = reader.GetString(reader.GetOrdinal("GroupName"));
                            string tags = reader.GetString(reader.GetOrdinal("Tags"));
                            string notes = reader.GetString(reader.GetOrdinal("Notes"));
                            int grams = reader.GetInt32(reader.GetOrdinal("grams"));
                            decimal ounces = reader.GetDecimal(reader.GetOrdinal("ounces"));    //calculated: grams * 28.3495 (This i grams per ounce.  0.035274 is oz/gm)
                            int lb = reader.GetInt32(reader.GetOrdinal("lb"));                //calculated: ounces % 16
                            decimal oz = reader.GetDecimal(reader.GetOrdinal("oz"));            //calculated: ounces - ((ounces % 16) * 16)
                            bool _new = reader.GetBoolean(reader.GetOrdinal("New"));
                            bool selected = reader.GetBoolean(reader.GetOrdinal("Selected"));
                            int ListOrder = reader.GetInt32(reader.GetOrdinal("ListOrder"));
                            PackItemsLO.Add(new PackItemsLO(id, groupName, item, grams, ounces, lb, oz, _new, selected, tags, notes, ListOrder));
                        }
                    }
                }
            }

            return PackItemsLO;
        }
    }
}
