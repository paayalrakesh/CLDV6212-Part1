using ABC_Retailers.Models;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABC_Retailers.Controllers
{
    public class TransactionController : Controller
    {

        private readonly QueueService _queueService;

        public TransactionController(QueueService queueService)
        {
            _queueService = queueService;
        }

        public IActionResult Transaction()
        {
            return View();
        }
        public IActionResult Index()
        {
            // Optionally, provide a view to list or manage transactions
            return View();
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        /*public async Task<IActionResult> CreateT(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                // Serialize transaction to JSON
                var message = JsonConvert.SerializeObject(transaction);
                await _queueService.SendMessageAsync(message);
                //return RedirectToAction(nameof(Details));
                return View(Details(transaction.TransactionId));
            }
            return View(transaction);
        }*/

        public async Task<IActionResult> ProcessQueue()
        {
            // Example of processing messages from the queue
            string messageText;
            while ((messageText = await _queueService.ReceiveMessageAsync()) != null)
            {
                // Deserialize and process the transaction
                var transaction = JsonConvert.DeserializeObject<Transaction>(messageText);
                // Process the transaction here
            }
            return View();
        }
        public IActionResult Edit(string id)
        {
            // Fetch transaction by id
            var transaction = new Transaction(); // Replace with actual data fetching logic
            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                // Update transaction logic
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        public IActionResult Delete(string id)
        {
            // Fetch transaction by id
            var transaction = new Transaction(); // Replace with actual data fetching logic
            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Delete transaction logic
            return RedirectToAction(nameof(Index));
        }

        public async Task <IActionResult> SaveDetails(Transaction transaction)
        {


           // if (ModelState.IsValid)

           // {
                // Serialize transaction to JSON
                var message = JsonConvert.SerializeObject(transaction);
                await _queueService.SendMessageAsync(message);
                return RedirectToAction(nameof(Details));          //  }
           
        }

        public async Task<IActionResult> Details(Transaction transaction) 
        { 
       
        return View(Details(transaction));
        
       
        }
    }
}

