using SCService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCService.Controllers
{
    public class ValuesController : ApiController
    {
        aldocosta01Entities db = new aldocosta01Entities();

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Registrando/Verificando registro de dispositivo
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public string GetVerifyuuid(string uuid)
        {

            var device = db.DEVICE.Where(p => p.uuid == uuid).FirstOrDefault();
            string action = "Device registrado";
            if (device == null)
            {
                var newdevice = new DEVICE();
                newdevice.dateRegistro = DateTime.Now;
                newdevice.uuid = uuid;
                db.DEVICE.Add(newdevice);
                db.SaveChanges();
                action = "Novo Device registrado";
            }

            return action;
        }


        public string GetListByHashCode(string hashCode)
        {
            var ret = db.LISTA_CONTATOS.Where(p => p.hashcode == hashCode).FirstOrDefault();

            if (ret != null)
            {
                return ret.contact_list;
            }
            return null;
        }

        //[HttpGet, ActionName("Me")]
        public string GetMe(string id)
        {
            return id;
        }

        // POST api/values
        public HttpResponseMessage PostContacts([FromBody]CS contact)
        {
            ClearBase();
            var hashcode = DateTime.Now.ToLongTimeString().GetHashCode();
            var datacriacao = DateTime.Now;

            LISTA_CONTATOS lc = new LISTA_CONTATOS();
            lc.uuid = contact.uuid;
            lc.contact_list = contact.contactListJson;
            lc.hashcode = hashcode.ToString();
            lc.dtCriacao = datacriacao;
            db.LISTA_CONTATOS.Add(lc);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, hashcode.ToString());
        }

        private void ClearBase()
        {
            var horaagora = DateTime.Now;

            var ret = db.LISTA_CONTATOS;

            foreach (var item in ret)
            {
                var retorno = horaagora - item.dtCriacao;
                if (retorno.Value.Days >= 1)
                {
                    db.LISTA_CONTATOS.Remove(item);
                }
            }
            db.SaveChanges();
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}