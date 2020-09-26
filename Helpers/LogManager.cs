using System.ComponentModel.Design;
using System.Runtime.Loader;
using System.Diagnostics.Contracts;
using System.ComponentModel.Design.Serialization;
using System.Threading;
using System.Data;
using System;
using System.Threading.Tasks;
using DocumentGenAPI.Entities;
using DocumentGenAPI.Context;


namespace DocumentGenAPI.Helpers
{
    public class LogManager : ILogManager
    {
        private readonly ApplicationDbContext context;

        public LogManager(ApplicationDbContext _context)
            {
                context = _context;
            }
        public async void CreateLog(string EventArgument, string EventContext)
        {
        var _event = new ExpenseLog{
                EventArgument = EventArgument,
                EventContext = EventContext,
                EventDateGeneration = DateTime.Today

            };
            context.Add(_event);
            await context.SaveChangesAsync();
    
        }
    }
    public interface ILogManager
    {
        void CreateLog(string EventArgument, string EventContext);
    }
}