using ApisDotnetCore6.Data;
using ApisDotnetCore6.Exception;
using ApisDotnetCore6.Helper;
using ApisDotnetCore6.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
        return _mapper.Map<Book>(bookRequest);
    }
}