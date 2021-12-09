using DemoDataManager.Library.Internal.DataAccess;
using DemoDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            SQLDataAccess sql = new SQLDataAccess();

            var p = new { Id = Id } ;
            
            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookUp", p, "DemoData");

            return output;
        }
    }
}
