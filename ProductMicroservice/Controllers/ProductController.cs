using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.Models;
using ProductMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Transactions;
/// <summary>
/// Product description
/// ProductMicroservice is based on .NET5 (.NET Core 5) With CRUD properties connected to SQL server DB
/// DataAccess: Repository DBContexts implemented with, Swagger, Dockerize (Docker, Container) CQRS, Mediator, RabbitMQ,
/// Methods: Get (Read), Post (Insert), Put (update) Delete, All Methods uses CQRS and mediator to distingish Read and Write 
/// Operations in the Database.
/// </summary>

namespace ProductMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        ///     Action to retrieve all Products.
        /// </summary>
        /// <returns>Returns a list of all Products or an empty list, if no Product is exist</returns>
        /// <response code="200">Returned if the list of Products was retrieved</response>
        /// <response code="400">Returned if the Products could not be retrieved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts();
            return new OkObjectResult(products);
        }


        /// <summary>
        ///     Action Get a Product.
        /// </summary>
        /// <param name="productId">The productId is a Prouct which should be retaind from DB </param>
        /// <returns>Returns is OK </returns>
        /// <response code="200">Returned if the Products has been found and retained </response>
        /// <response code="400">Returned if the Prodcut could not be found to retaind with ProductId</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var product = _productRepository.GetProductByID(id);
            return new OkObjectResult(product);
        }

        /// <summary>
        /// Action: Post to create a new product in the database.
        /// </summary>
        /// <param name="product">Model to create a new Product</param>
        /// <returns>Returns the created product</returns>
        /// <response code="200">Returned if the product was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or the product couldn't be saved</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            using (var scope = new TransactionScope())
            {
                _productRepository.InsertProduct(product);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
        }

        /// <summary>
        ///  Action: Put to update a Product in the Database
        /// </summary>
        /// <param name="product">The product is a Prouct which should be updated in DB </param>
        /// <returns>Returns is OK </returns>
        /// <response code="200">Returned if the Product was updated </response>
        /// <response code="400">Returned if the Prodcut could not be found with Product.Id</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("Product")]

        [HttpPut]
        public IActionResult Put([FromBody] Product product)
        {
            if (product != null)
            {
                using (var scope = new TransactionScope())
                {
                    _productRepository.UpdateProduct(product);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        /// <summary>
        ///   Action Delete: to delete a Product on the Database.
        /// </summary>
        /// <param name="productId">The productId is a Prouct which should be deleted from DB </param>
        /// <returns>Returns is OK </returns>
        /// <response code="200">Returned if the Product has been found and deleted </response>
        /// <response code="400">Returned if the Prodcut could not be found for  deletion with ProductId</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{ld}", Name = "Delete")]

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productRepository.DeleteProduct(id);
            return new OkResult();
        }
    }
}