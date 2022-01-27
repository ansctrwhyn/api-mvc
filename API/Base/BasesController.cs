using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController<Entity, Repository, Key> : Controller
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;
        public BasesController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<Entity> Get()
        {
            var result = repository.Get();
            if (result.Count() > 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil ditampilkan" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
        }

        [HttpGet("{key}")]
        public ActionResult Get(Key key)
        {
            var result = repository.Get(key);
            if (result != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil ditampilkan" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
        }

        [HttpPost]
        public virtual ActionResult Post(Entity entity)
        {
            var result = repository.Insert(entity);
            if (result != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil ditambahkan" });
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Gagal ditambahkan" });
            }
        }

        [HttpPut]
        public ActionResult Put(Entity entity)
        {
            var result = repository.Update(entity);
            if (result != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil diubah" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }

        }

        [HttpDelete("{key}")]
        public ActionResult Delete(Key key)
        {
            var result = repository.Delete(key);
            if (result != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil dihapus" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
        }
    }
}
