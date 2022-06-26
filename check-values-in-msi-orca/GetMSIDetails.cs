public static Boolean GetMsiDetails(ref ProductDetails productDetails, string msiPath, List<string> properties, string tableName)
        {
            try
            {
                using (var database = new Microsoft.Deployment.WindowsInstaller.Database(msiPath))
                {
                    var sql = "SELECT * FROM " + tableName;
                    using (var view = database.OpenView(sql))
                    {
                        view.Execute();
                        var columnsName = view.Columns.ToList();
                        var ProductProperties = view.ToList();
                        foreach (var prop in properties)
                        {
                            foreach (var property in ProductProperties)
                            {
                                if (property.GetString(columnsName[0].Name) == prop)
                                {
                                    Type type = typeof(ProductDetails);
                                    PropertyInfo objectProperty = type.GetProperty(prop);
                                    objectProperty.SetValue(productDetails, property.GetString(columnsName[1].Name), null);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }