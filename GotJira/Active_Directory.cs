using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace GotJira
{
    public class Active_Directory
    {
        public List<UserAD> GetADUsers()
        {
            List<UserAD> rst = new List<UserAD>();
            //<add name="ApplicationServices" connectionString="LDAP://softoffice.com.ar:389|softoffice.com.ar\*|True|DC=softoffice,DC=com,DC=ar"/>                       
            string DomainPath = "LDAP://softoffice.com.ar:389/DC=softoffice,DC=com,DC=ar";
            DirectoryEntry adSearchRoot = new DirectoryEntry(DomainPath);
            DirectorySearcher adSearcher = new DirectorySearcher(adSearchRoot);

            adSearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
            adSearcher.PropertiesToLoad.Add("samaccountname");
            adSearcher.PropertiesToLoad.Add("title");
            adSearcher.PropertiesToLoad.Add("mail");
            adSearcher.PropertiesToLoad.Add("usergroup");
            adSearcher.PropertiesToLoad.Add("company");
            adSearcher.PropertiesToLoad.Add("department");
            adSearcher.PropertiesToLoad.Add("telephoneNumber");
            adSearcher.PropertiesToLoad.Add("mobile");
            adSearcher.PropertiesToLoad.Add("displayname");
            SearchResult result;
            SearchResultCollection iResult = adSearcher.FindAll();

            UserAD item;
            if (iResult != null)
            {
                for (int counter = 0; counter < iResult.Count; counter++)
                {
                    result = iResult[counter];
                    if (result.Properties.Contains("samaccountname"))
                    {
                        item = new UserAD();

                        item.UserName = (String)result.Properties["samaccountname"][0];

                        if (result.Properties.Contains("displayname"))
                        {
                            item.DisplayName = (String)result.Properties["displayname"][0];
                        }

                        if (result.Properties.Contains("mail"))
                        {
                            item.Email = (String)result.Properties["mail"][0];
                        }

                        if (result.Properties.Contains("company"))
                        {
                            item.Company = (String)result.Properties["company"][0];
                        }

                        if (result.Properties.Contains("title"))
                        {
                            item.JobTitle = (String)result.Properties["title"][0];
                        }

                        if (result.Properties.Contains("department"))
                        {
                            item.Deparment = (String)result.Properties["department"][0];
                        }

                        if (result.Properties.Contains("telephoneNumber"))
                        {
                            item.Phone = (String)result.Properties["telephoneNumber"][0];
                        }

                        if (result.Properties.Contains("mobile"))
                        {
                            item.Mobile = (String)result.Properties["mobile"][0];
                        }
                        rst.Add(item);
                    }
                }
            }

            adSearcher.Dispose();
            adSearchRoot.Dispose();

            //foreach (UserAD usr in rst) {
            //    if (usr.DisplayName == "Antonio Clavero - Emerix") {
            //        System.Diagnostics.Debugger.Break();
            //    }
            //}
            return rst;            
        }
        public class UserAD
        {
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string Company { get; set; }
            public string Deparment { get; set; }
            public string JobTitle { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Mobile { get; set; }
        }
    }
}
