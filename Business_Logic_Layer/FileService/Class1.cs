namespace Business_Logic_Layer.DbUpdate
{
    public class Class1
    {
        public static Dictionary<string, string> LoadUpdateScripts(string mydocpath)
        {
            var updateScriptList = new Dictionary<string, string>();

            foreach (string txtName in Directory.GetFiles(mydocpath, "*.sql"))
            {
                using (StreamReader sr = new StreamReader(txtName))
                {
                    string fileText = sr.ReadToEnd();
                    updateScriptList.Add(txtName.ToString(), fileText);
                }
            }

            return updateScriptList;
        }
    }
}
