using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model = Inmobiliaria.Client.Controller.ServiceInmobiliaria;

namespace Inmobiliaria.Client.Controller
{
    public static class LocalDataStore
    {
        //static ServicesManager _serviceManager = ServicesManager.Instance; 

        static List<Model.Edificio> _listEdificios;
        public static List<Model.Edificio> ListEdificios
        {
            get
            {
                if (_listEdificios == null)
                    _listEdificios = ServicesManager.Instance.ServiceClient.GetTodosLosEdificios().ToList();
                return _listEdificios;
            }
        }

        public static bool modificarEdificio(Model.Edificio edificio)
        {
            Model.Edificio tempEdificio = new Model.Edificio()
            {
                Id = edificio.Id,
                Nombre = edificio.Nombre,
                N_Plantas = edificio.N_Plantas,
                mainfoto = edificio.mainfoto,
                A_Contruccion = edificio.A_Contruccion,
                Inf_Adicional = edificio.Inf_Adicional,
                Id_Ubi_Detalle = edificio.Id_Ubi_Detalle            
            };
             bool response = ServicesManager.Instance.ServiceClient.ModificarEdificio(tempEdificio);
             if (response)
             {
                Model.Edificio copyedificio = _listEdificios.Find(P =>P.Id == edificio.Id);
                copyedificio = edificio; 
             }
             return false;
        }

        public static bool GuardarEdificio(Model.Edificio edificio, string pathImageOrigen)
        {
            string response = ServicesManager.Instance.ServiceClient.GuardarEdificio(edificio, pathImageOrigen);
            if (response != "")
            {
                edificio.Id = response;
                _listEdificios.Add(edificio);
                return true;
            }
            return false;
        }

        public static bool EliminarEdificio(string Id_Edificio)
        {
            bool response = ServicesManager.Instance.ServiceClient.EliminarEdificio(Id_Edificio);
            if (response)
                _listEdificios.RemoveAll(P => P.Id == Id_Edificio);
            return response;
        }

        static Dictionary<Model.Ubicacion, List<Model.Ubicacion_Detalle>> _dicUbicaciones;

        public static List<Model.Ubicacion> ListUbicaciones
        {
            get
            {
                if (_dicUbicaciones == null)
                    _dicUbicaciones = ServicesManager.Instance.ServiceClient.GetDictionaryUbicaciones();

                return _dicUbicaciones.Keys.ToList();
            }

        }

        public static List<Model.Ubicacion_Detalle> GetUbicacionDetalle(Model.Ubicacion ubicacion)
        {
            List<Model.Ubicacion_Detalle> result = _dicUbicaciones.First(U => U.Key.Id == ubicacion.Id).Value;
            return result;
        }



    }
}
