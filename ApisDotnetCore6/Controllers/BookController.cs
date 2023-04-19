using System.Net;
using ApisDotnetCore6.Data;
using ApisDotnetCore6.Exception;
using ApisDotnetCore6.Models;
using ApisDotnetCore6.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApisDotnetCore6.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : Controller
{
    private readonly BookService _bookService;

    public BookController(BookService bookService)
    {
        this._bookService = bookService;
    }

    [HttpGet]
    public async Task<List<Book>> GetBooks()
    {
        return await _bookService.GetAllBooks();
    }

    [HttpGet("/{id}")]
    public async Task<Book> GetBookById([FromRoute] int id)
    {
        try
        {
            return await _bookService.GetBookById(id);
        }
        catch (NotFoundResourceException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("/{id}")]
    public async Task<ObjectResult> DeleteBookById([FromRoute] int id)
    {
        await _bookService.DeleteBookById(id);

        return await Task.FromResult(
            new ObjectResult(StatusCode((int)HttpStatusCode.OK,
                $"Deleted id = {id}")));
    }

    [HttpPost]
    public async Task<ObjectResult> SaveBook([FromBody] BookModel bookRequest)
    {
        return await Task.FromResult(
            new ObjectResult(
                StatusCode((int)HttpStatusCode.Created,
                    _bookService.SaveBook(bookRequest).Result)));
    }

    [HttpPut]
    public async Task<ObjectResult> PutBook([FromBody] BookModel bookRequest)
    {
        return await Task.FromResult(
            new ObjectResult(
                StatusCode((int)HttpStatusCode.OK,
                    _bookService.PutBook(bookRequest).Result)));
    }
}