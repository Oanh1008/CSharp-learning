using ApisDotnetCore6.Data;
using ApisDotnetCore6.Exception;
using ApisDotnetCore6.Helper;
using ApisDotnetCore6.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ApisDotnetCore6.Service;

public class BookService
{
    private readonly BookStoreContext _bookStoreContext;

    private readonly IMapper _mapper;

    public BookService(BookStoreContext bookStoreContext, IMapper mapper)
    {
        _bookStoreContext = bookStoreContext;
        _mapper = mapper;
    }

    public async Task<List<Book>> GetAllBooks()
    {
        return await _bookStoreContext.Books!.ToListAsync();
    }

    public async Task<Book> GetBookById(int id)
    {
        var book = await _bookStoreContext.Books!.FindAsync(id);

        if (book?.Id == null)
        {
            throw new NotFoundResourceException("Resource not found !");
        }

        return book;
    }

    public async Task DeleteBookById(int id)
    {
        var deleteBook = _bookStoreContext.Books!.SingleOrDefault(book => book.Id == id);

        if (deleteBook == null)
        {
            throw new NotFoundResourceException("Resource not found !");
        }

        _bookStoreContext.Books!.Remove(deleteBook);
        await _bookStoreContext.SaveChangesAsync();
    }

    public async Task<Book> SaveBook(BookModel bookRequest)
    {
        var book = _mapper.Map<Book>(bookRequest);

        _bookStoreContext.Books!.Add(book);
        await _bookStoreContext.SaveChangesAsync();
        return book;
    }

    public async Task<BookModel> PutBook(BookModel bookRequest)
    {
        var book = await _bookStoreContext.Books!.FindAsync(bookRequest.Id);

        if (book == null)
        {
            throw new NotFoundResourceException($"The book with id = {bookRequest.Id} is not found !");
        }

        book.Title = bookRequest.Title;
        book.Description = bookRequest.Description;
        book.Price = book.Price;
        book.Quantity = bookRequest.Quantity;

        var checkTrack = _bookStoreContext.ChangeTracker.HasChanges();
        await _bookStoreContext.SaveChangesAsync();
        return _mapper.Map<BookModel>(book);
    }

    public Book DemoOutKeyword(BookModel bookRequest, out string output)
    {
        output = $"Test string {bookRequest.Quantity}";

        var books = from s in _bookStoreContext.Books select s;
        var book = _bookStoreContext.Books!.AsQueryable();
        return _mapper.Map<Book>(bookRequest);
    }

    public async Task<PagingResponseModel<BookModel>> PagingFilter(PagingRequestModel pagingRequestModel)
    {
        PagingUtils.ValidatePageRequest(pagingRequestModel);

        var pageNumber = pagingRequestModel.PageNumber;
        var pageSize = pagingRequestModel.PageSize;
        var title = pagingRequestModel.Title;
        var description = pagingRequestModel.Description;
        var price = pagingRequestModel.Price;
        var sortBy = pagingRequestModel.SortBy;
        var orderBy = pagingRequestModel.OrderBy;

        // Filtering
        var iBooks = _bookStoreContext.Books!.Where(book =>
            (string.IsNullOrEmpty(title) || book.Title!.ToLower().Contains(title.ToLower()))
            && (string.IsNullOrEmpty(description) || book.Description!.ToLower().Contains(description.ToLower()))
            && (price == 0 || price == book.Price)
        );

        // Handling sorting ascending or descending
        iBooks = (sortBy, orderBy) switch
        {
            ("Id", "asc") => iBooks.OrderBy(book => book.Id),
            ("Id", "desc") => iBooks.OrderByDescending(book => book.Id),
            ("Title", "asc") => iBooks.OrderBy(book => book.Title),
            ("Title", "desc") => iBooks.OrderByDescending(book => book.Title),
            ("Description", "asc") => iBooks.OrderBy(book => book.Description),
            ("Description", "desc") => iBooks.OrderByDescending(book => book.Description),
            ("Price", "asc") => iBooks.OrderBy(book => book.Price),
            ("Price", "desc") => iBooks.OrderByDescending(book => book.Price),
            ("Quantity", "asc") => iBooks.OrderBy(book => book.Quantity),
            ("Quantity", "desc") => iBooks.OrderByDescending(book => book.Quantity),
            _ => iBooks
        };

        // Get number of record after filtering
        var totalRecord = iBooks.Count();

        // Pagination
        var books = await iBooks
            .Skip((pageNumber - 1)! * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

        return new PagingResponseModel<BookModel>(
            _mapper.Map<List<BookModel>>(books), pageNumber, pageSize, totalRecord, totalPage,
            sortBy ?? string.Empty, orderBy ?? string.Empty);
    }
}