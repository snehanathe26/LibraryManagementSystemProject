
using LibraryManagement.Entity;
using LibraryManagement.Model;
using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel;
using Microsoft.Azure.Cosmos;


namespace LibraryManagement.Controllers
{
    
    [Route("api/[Controller]/[Action]")]
    [ApiController]

    public class LibraryController : Controller
    {
        public string URI = "https://localhost:8081";
        public string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public string ContainerName = "BookStorage";
        public string DatabaseName = "Library";

        public Container container;
        public Container GetContainer()
        {
            CosmosClient com = new CosmosClient(URI, PrimaryKey);
            Database database = com.GetDatabase(DatabaseName);
            container = database.GetContainer(ContainerName);
            return container;
        }
        public LibraryController()
        {

            container = GetContainer();
        }

        //BookOperation
        [HttpPost]

        public async Task<BookModel> AddBooks(BookModel bookmodel)
        {
            BookEntity bookEntity = new BookEntity();
            //object and mapping
            bookEntity.Title = bookmodel.Title;
            bookEntity.Author = bookmodel.Author;
            bookEntity.PublishedDate = bookmodel.PublishedDate;
            bookEntity.ISBN = bookmodel.ISBN;
            bookEntity.IsIssued = bookmodel.IsIssued;

            // Assign values to the mandatory fields

            bookEntity.UId = Guid.NewGuid().ToString();
            bookEntity.Id = bookEntity.UId;
            bookEntity.DocumentType = "RefereceBook";
            bookEntity.CreatedBy = "SnehaNathe";
            bookEntity.UpdatedBy = "XYZ ";
            bookEntity.CreatedOn = DateTime.Now;
            bookEntity.UpdatedOn = DateTime.Now;
            bookEntity.Version = 1;
            bookEntity.IsIssued = true;
            bookEntity.Active = true;
            bookEntity.Archived = false;

            //add to the database

            BookEntity Bookm = await container.CreateItemAsync(bookEntity);
            //return
            


            BookModel Response = new BookModel();
            Response.Title = Bookm.Title;
            Response.Author = Bookm.Author;
            Response.PublishedDate = Bookm.PublishedDate;
            Response.IsIssued = Bookm.IsIssued;
            Response.ISBN = Bookm.ISBN;
            return Response;
        }
        [HttpGet("{Title}")]

        public  async Task<BookModel>GetBookByTitle(string title)
        {
            var Book = container.GetItemLinqQueryable<BookEntity>(true).Where(q => q.Title == title && q.Active == true && q.Archived == false).FirstOrDefault();
            BookModel bookmodel = new BookModel();
            bookmodel.UId = Book.UId;
            bookmodel.Title = Book.Title;
            bookmodel.Author = Book.Author;
            bookmodel.PublishedDate = Book.PublishedDate;
            bookmodel.ISBN = Book.ISBN;
            bookmodel.IsIssued = Book.IsIssued;
            return bookmodel;
        }

        [HttpGet]
        public async Task<List<BookModel>> GetAllBooks()
        {
            var Books = container.GetItemLinqQueryable<BookEntity>(true).Where(q => q.Active == true && q.Archived == false && q.DocumentType == "Book").ToList();
            List<BookModel> bookModels = new List<BookModel>();
            for (int i = 0; i < Books.Count(); i++)
            {
                BookModel bookmodel = new BookModel();
                bookmodel.UId = Books[i].UId;
                bookmodel.Title = Books[i].Title;
                bookmodel.PublishedDate = Books[i].PublishedDate;
                bookmodel.ISBN = Books[i].ISBN;
                bookmodel.IsIssued = Books[i].IsIssued;
                bookModels.Add(bookmodel);
            }
            return bookModels;
        }

        [HttpGet]
        public async Task<List<BookModel>> GetAllNonIssuedBook()
        {
            var Books = container.GetItemLinqQueryable<BookEntity>(true)
                .Where(q => q.IsIssued == false && q.Active == true &&
            q.Archived == false && q.DocumentType == "Book").ToList();

            List<BookModel> bookModels = new List<BookModel>();
            for (int i = 0; i < Books.Count(); i++)
            {
                BookModel bookmodel = new BookModel();
                bookmodel.UId = Books[i].UId;
                bookmodel.Title = Books[i].Title;
                bookmodel.Author = Books[i].Author;
                bookmodel.PublishedDate = Books[i].PublishedDate;
                bookmodel.IsIssued = Books[i].IsIssued;
                bookModels.Add(bookmodel);
            }
            return bookModels;
        }
        [HttpGet]
        public async Task<List<BookModel>> GetAllIssuedBook()
        {
            var Books = container.GetItemLinqQueryable<BookEntity>(true)
               .Where(q => q.IsIssued == false && q.Active == true &&
           q.Archived == false && q.DocumentType == "Book").ToList();

            List<BookModel> bookModels = new List<BookModel>();
            for (int i = 0; i < Books.Count(); i++)
            {
                BookModel bookmodel = new BookModel();
                bookmodel.UId = Books[i].UId;
                bookmodel.Title = Books[i].Title;
                bookmodel.Author = Books[i].Author;
                bookmodel.PublishedDate = Books[i].PublishedDate;
                bookmodel.IsIssued = Books[i].IsIssued;
                bookModels.Add(bookmodel);
            }
            return bookModels;
        }

        [HttpPost]
        public async Task<BookModel> UpdateBook(BookModel bookmodel)
        {
            //get the existing record by UId
            var existingBook = container.GetItemLinqQueryable<BookEntity>(true)
                .Where(q => q.UId == bookmodel.UId && q.Active == true && q.Archived == false).FirstOrDefault();
            //Replace the records
            existingBook.Archived = true;
            existingBook.Active = false;
            await container.ReplaceItemAsync(existingBook, existingBook.Id);

            //4.Assign the values to the fields which we will get from request obj
            existingBook.Title = bookmodel.Title;
            existingBook.Author = bookmodel.Author;
            existingBook.ISBN = bookmodel.ISBN;
            existingBook.IsIssued = bookmodel.IsIssued;
            existingBook.PublishedDate = bookmodel.PublishedDate;


            //5.Add the date to the database

            existingBook = await container.CreateItemAsync(existingBook);

            //return
            BookModel response = new BookModel();
            response.UId = existingBook.UId;
            response.Title = existingBook.Title;
            response.Author = existingBook.Author;
            response.PublishedDate = existingBook.PublishedDate;
            response.ISBN = existingBook.ISBN;
            response.IsIssued = existingBook.IsIssued;
            return response;

        }
        //member operation


        [HttpPost]
        public async Task<MemberModel> AddMembers(MemberModel membermodel)
        {

            //Object and Mapping
            MemberEntity memberEntity = new MemberEntity();
            memberEntity.Name = membermodel.Name;
            memberEntity.DateOfBirth = membermodel.DateOfBirth;
            memberEntity.Email = membermodel.Email;

            //Assign values to madatory fields
            memberEntity.UId = Guid.NewGuid().ToString();
            memberEntity.Id = memberEntity.UId;
            memberEntity.DocumentType = "Member";
            memberEntity.CreatedBy = "Member";
            memberEntity.CreatedOn = DateTime.Now;
            memberEntity.UpdatedBy = "";
            memberEntity.UpdatedOn = DateTime.Now;
            memberEntity.Version = 1;
            memberEntity.Active = true;
            memberEntity.Archived = false;

            //Add to database 
            MemberEntity Response = await container.CreateItemAsync(memberEntity);


            //return
            MemberModel MemberM = new MemberModel();
            MemberM.Name = Response.Name;
            MemberM.Email = Response.Email;
            MemberM.DateOfBirth = Response.DateOfBirth;
            return MemberM;
        }

        [HttpGet("{Uid}")]
        public async Task<MemberModel> GetMemberByUid(string Uid)
        {
            var member = container.GetItemLinqQueryable<MemberEntity>(true).Where(q => q.UId == Uid && q.Active == true && q.Archived == false && q.DocumentType == "Member").FirstOrDefault();
            MemberModel MemberM = new MemberModel();
            MemberM.Name = member.Name;
            MemberM.Email = member.Email;
            MemberM.DateOfBirth = member.DateOfBirth;
            return MemberM;
        }

        [HttpGet]
        public async Task<List<MemberModel>> GetAllMembers()
        {
            var Members = container.GetItemLinqQueryable<MemberEntity>(true).Where(q => q.Active == true && q.Archived == false && q.DocumentType == "Member").ToList();
            List<MemberModel> MemberModels = new List<MemberModel>();
            for (var i = 0; i < Members.Count; i++)
            {
                MemberModel MemberModel = new MemberModel();
                MemberModel.UId = Members[i].UId;
                MemberModel.Name = Members[i].Name;
                MemberModel.Email = Members[i].Email;
                MemberModel.DateOfBirth = Members[i].DateOfBirth;
                MemberModels.Add(MemberModel);
            }
            return MemberModels;
        }

        [HttpPost]
        public async Task<MemberModel> UpdateMember(MemberModel memberModel)
        {   //fetch
            var existingMember = container.GetItemLinqQueryable<MemberEntity>(true).Where(q => q.UId == memberModel.UId && q.Active == true && q.Archived == false).FirstOrDefault();

            //replace
            existingMember.Archived = true;
            existingMember.Active = false;
            await container.ReplaceItemAsync(existingMember, existingMember.Id);

            //Assign
            existingMember.Id = Guid.NewGuid().ToString();
            existingMember.UpdatedBy = "SnehaNathe";
            existingMember.UpdatedOn = DateTime.Now;
            existingMember.Version = ++existingMember.Version;
            existingMember.Active = true;
            existingMember.Archived = false;

            //Updation field
            existingMember.Name = memberModel.Name;
            existingMember.Email = memberModel.Email;
            existingMember.DateOfBirth = memberModel.DateOfBirth;

            //push to database
            existingMember = await container.CreateItemAsync(existingMember);

            //return
            MemberModel Response = new MemberModel();
            Response.UId = existingMember.UId;
            Response.Name = existingMember.Name;
            Response.Email = existingMember.Email;
            return Response;
        }

        //Issue OPERATION

        [HttpPost]
        public async Task<IssueModel> IssueBooks(IssueModel issueModel)
        {
            //Object and Mapping
            IssueEntity issueEntity = new IssueEntity();
            issueEntity.BookId = issueModel.BookId;
            issueEntity.MemberId = issueModel.MemberId;
            issueEntity.IssueDate = issueModel.IssueDate;
            //issueEntity.ReturnDate=issueModel.ReturnDate;
            issueEntity.IsReturned = issueModel.IsReturned;

            //Assign values to madatory fields
            issueEntity.UId = Guid.NewGuid().ToString();
            issueEntity.Id = issueEntity.UId;
            issueEntity.DocumentType = "IssueBook";
            issueEntity.CreatedBy = "Ashwini";
            issueEntity.CreatedOn = DateTime.Now;
            issueEntity.UpdatedBy = "";
            issueEntity.UpdatedOn = DateTime.Now;
            issueEntity.Version = 1;
            issueEntity.Active = true;
            issueEntity.Archived = false;

            //Add to database 
            IssueEntity Response = await container.CreateItemAsync(issueEntity);

            //Return
            IssueModel IssueM = new IssueModel();
            IssueM.BookId = Response.BookId;
            IssueM.MemberId = Response.MemberId;
            IssueM.IssueDate = Response.IssueDate;
            // IssueM.ReturnDate = Response.ReturnDate;
            IssueM.IsReturned = Response.IsReturned;
            return IssueM;
        }

        [HttpGet("{Uid}")]
        public async Task<IssueModel> GetIssueByUid(string Uid)
        {
            var issuebook = container.GetItemLinqQueryable<IssueEntity>(true).Where(q => q.UId == Uid && q.Active == true && q.Archived == false && q.DocumentType == "IssueBook").FirstOrDefault();
            var book = container.GetItemLinqQueryable<BookEntity>(true).Where(q => q.Id == issuebook.BookId && q.Active == true && q.Archived == false && q.DocumentType == "Book").FirstOrDefault();
           
            IssueModel issueM = new IssueModel();

            issueM.BookId = issuebook.BookId;
            issueM.MemberId = issuebook.MemberId;
            issueM.IssueDate = issuebook.IssueDate;
            issueM.ReturnDate = issuebook.ReturnDate;
            issueM.IsReturned = issuebook.IsReturned;
            return issueM;

        }
        [HttpGet("{Uid}")]
        public async Task<IssueModel> GetIssueBookByUid(string Uid)
        {
            var issuebook = container.GetItemLinqQueryable<IssueEntity>(true).Where(q => q.UId == Uid && q.Active == true && q.Archived == false && q.DocumentType == "IssueBook").FirstOrDefault();
            var book = container.GetItemLinqQueryable<BookEntity>(true).Where(q => q.UId == issuebook.BookId && q.Active == true && q.Archived == false && q.DocumentType == "Book").FirstOrDefault();
            IssueModel issueM = new IssueModel();
            issueM.BookId = issuebook.BookId;
            return issueM;
        }

        [HttpPost]
        public async Task<IssueModel> updateexistingissue(IssueModel issueModel)
        {
            //Fetch
            var existingissue = container.GetItemLinqQueryable<IssueEntity>(true).Where(q => q.UId == issueModel.UId && q.Active == true && q.Archived == false).FirstOrDefault();
            //Replace
            existingissue.Archived = true;
            existingissue.Active = false;
            await container.ReplaceItemAsync(existingissue, existingissue.Id);
            //Assign
            existingissue.Id = Guid.NewGuid().ToString();
            existingissue.UpdatedBy = "SnehaNathe";
            existingissue.UpdatedOn = DateTime.Now;
            existingissue.Version = ++existingissue.Version;
            existingissue.Active = true;
            existingissue.Archived = false;
            //Updatation field
            existingissue.BookId = issueModel.BookId;
            existingissue.MemberId = issueModel.MemberId;
            existingissue.IssueDate = issueModel.IssueDate;
            existingissue.ReturnDate = issueModel.ReturnDate;
            existingissue.IsReturned = issueModel.IsReturned;
            //Push to Database
            existingissue = await container.CreateItemAsync(existingissue);
            //return
            IssueModel Response = new IssueModel();
            Response.UId = existingissue.UId;
            Response.BookId = existingissue.BookId;
            Response.MemberId = existingissue.MemberId;
            Response.IssueDate = existingissue.IssueDate;
            Response.ReturnDate = existingissue.ReturnDate;
            Response.IsReturned = existingissue.IsReturned;
            return Response;
        }

    }
}
