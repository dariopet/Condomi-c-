using Foolproof;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication

{

    public class AdelantosMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
        [Required(ErrorMessage = "Ingrese un monto")]
        [Range(0.1, Double.MaxValue, ErrorMessage = "El valor tiene que ser superior a 0")]
        public decimal monto;
        [Required(ErrorMessage = "Ingrese un fecha")]
        public System.DateTime fecha;
    }

    public class ValesMetadata
    {
        [Required(ErrorMessage = "Ingrese un monto")]
        [Range(0.1, Double.MaxValue, ErrorMessage = "El valor tiene que ser superior a 0")]
        public decimal monto;      
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
    }

    public class MantenimientoPreventivoMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
        [Required(ErrorMessage = "Ingrese un fechaGenerado")]
        public System.DateTime fechaGenerado;
        [Required(ErrorMessage = "Ingrese un fechaLimite")]
        public System.DateTime fechaLimite;
    }

    public class DetalleMantenimientoPreventivoMetadata
    {
        [Required(ErrorMessage = "Ingrese un estado")]
        public int estado;
    }



    public class ProximasComprasMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
        [Required(ErrorMessage = "Ingrese un monto")]
        [Range(0.1, Double.MaxValue, ErrorMessage = "El valor tiene que ser superior a 0")]
        public decimal monto;
    }

    public class EquiposMetadata
    {
        [Required(ErrorMessage = "Ingrese un código de barras")]
        public string codigoBarras;
    }

    [Serializable]
    public class TercerosMetadata
    {
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string nombre;
        [Required(ErrorMessage = "Ingrese un saldo")]
        public decimal saldo;

        [JsonIgnore]
        public virtual ICollection<Cheques> Cheques { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cuentas> Cuentas { get; set; }
        [JsonIgnore]
        public virtual ICollection<LogCobranzas> LogCobranzas { get; set; }
        [JsonIgnore]
        public virtual TipoTerceros TipoTerceros { get; set; }
        [JsonIgnore]
        public virtual ICollection<DetallePresupuesto> DetallePresupuesto { get; set; }
        [JsonIgnore]
        public virtual ICollection<HorasTrabajadasTecnico> HorasTrabajadasTecnico { get; set; }
        [JsonIgnore]
        public virtual ICollection<Egresos> Egresos { get; set; }
        [JsonIgnore]
        public virtual ICollection<Eventos> Eventos { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ingresos> Ingresos { get; set; }
        [JsonIgnore]
        public virtual ICollection<HorasNoTrabajadas> HorasNoTrabajadas { get; set; }
        [JsonIgnore]
        public virtual ICollection<Sueldos> Sueldos { get; set; }

    }
    public enum Estados
    {
        [Display(Name = "En depósito")]
        Deposito,
        [Display(Name = "En evento")]
        Trabajando,
        [Display(Name = "Fuera de servicio")]
        Roto,
        [Display(Name = "En reparación")]
        Reparación
    }
    public enum tipoMovimiento
    {
        [Display(Name = "Sale a evento")]
        aEvento = 0,
        [Display(Name = "Vuelve a depósito")]
        vuelveDesposito = 1,
        [Display(Name = "Sale a reparación")]
        aReparacion = 2
    }

    public class CobrosMetadata
    {
        [Required(ErrorMessage = "Ingrese un fecha")]
        public System.DateTime fecha;
        [Required(ErrorMessage = "Ingrese un monto")]
        [Range(0.1, Double.MaxValue, ErrorMessage = "El valor tiene que ser superior a 0")]
        public decimal monto;
    }

    public class PagosMetadata
    {
        [Required(ErrorMessage = "Ingrese un fecha")]
        public System.DateTime fecha;
        [Required(ErrorMessage = "Ingrese un monto")]
        [Range(0.1, Double.MaxValue, ErrorMessage = "El valor tiene que ser superior a 0")]
       [GreaterThan("retenido", ErrorMessage = "El monto debe ser mayor a lo retenido")]
        public decimal monto { get; set; }
      
    }

    public class ChequesMetadata
    {
        [Required(ErrorMessage = "Ingrese un banco")]
        public string banco;
        [Required(ErrorMessage = "Ingrese una fecha de cobro")]
        public System.DateTime fechaCobro;
        [Required(ErrorMessage = "Ingrese un valor")]
        public bool cobrado;
        [Required(ErrorMessage = "Ingrese un monto")]
        public decimal monto;
        [Required(ErrorMessage = "Ingrese el número de cheque")]
        public string numCheque;
        [Required(ErrorMessage = "Ingrese si es en blanco")]
        public bool enBlanco;
    }


    public class ChequesPorEgresoMetadata
    {
    }


    public class ChequesPorIngresoMetadata
    {
    }


    public class CuentasMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
    }


    public class DetalleEgresosMetadata
    {
        [Required(ErrorMessage = "Ingrese un monto")]
        [Range(0.1, Double.MaxValue, ErrorMessage = "El valor tiene que ser superior a 0")]
        public decimal monto;
    }


    public class DetalleIngresosMetadata
    {
        [Required(ErrorMessage = "Ingrese un monto")]
        [Range(0.1, Double.MaxValue, ErrorMessage = "El valor tiene que ser superior a 0")]
        public decimal monto;
    }



    public class DetallePresupuestoMetadata
    {
        [Required(ErrorMessage = "Ingrese un precio x día")]
        public decimal precioDia;
        [Required(ErrorMessage = "Ingrese un precio x hora")]
        public decimal precioHora;
        [Required(ErrorMessage = "Ingrese un costo x hora")]
        public decimal costoHora;
        [Required(ErrorMessage = "Ingrese un costo x día")]
        public decimal costoDia;
        [Required(ErrorMessage = "Ingrese cuantas horas")]
        public int horas;
        [Required(ErrorMessage = "Ingrese cuantos días")]
        public int dias;
    }


    public class EgresosMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
        [Required(ErrorMessage = "Ingrese un número de factura")]
        public string factura;
        [Required(ErrorMessage = "Ingrese un tipo de factura")]
        public tiposFacturas tipoFactura;
        [Required(ErrorMessage = "Ingrese una fecha de vencimiento")]
        public System.DateTime fechaVencimiento;
        [Required(ErrorMessage = "Ingrese si es en blanco")]
        public bool enBlanco;
        [Required(ErrorMessage = "Ingrese una fecha de emitida")]
        public System.DateTime fechaGenerada;
        [Required(ErrorMessage = "Ingrese un plan de cuenta")]
        public int idPlanCuentas;

        [Required(ErrorMessage = "Ingrese un valor")]
        public decimal impuestosInternos;
        [Required(ErrorMessage = "Ingrese un valor")]
        public decimal ingresosBrutos;
        [Required(ErrorMessage = "Ingrese un valor")]
        public decimal iva;
        [Required(ErrorMessage = "Ingrese un valor")]
        public decimal percepcionIva;
    }




    public class EventosMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
        [Required(ErrorMessage = "Ingrese una fecha de inicio")]
        public System.DateTime fechaInicio;
        [Required(ErrorMessage = "Ingrese una fecha de finalización")]
        [GreaterThan("fechaInicio", ErrorMessage = "La fecha de finalización debe ser posterior a la de inicio")]
        public System.DateTime fechaFin { get; set; }
        [Required(ErrorMessage = "Ingrese un estado")]
        public int estado;
        [Required(ErrorMessage = "Ingrese una jurisdicción")]
        public string jurisdiccion;





    }


    public class HorasNoTrabajadasMetadata
    {
        [Required(ErrorMessage = "Ingrese un periodo")]
        public string periodo;
        [Required(ErrorMessage = "Ingrese una fecha")]
        public System.DateTime fecha;
    }


    public class HorasTrabajadasTecnicoMetadata
    {
        [Required(ErrorMessage = "Ingrese una fecha")]
        public System.DateTime fecha;
        [Required(ErrorMessage = "Ingrese una cantidad")]
        public decimal cantidad;
        [Required(ErrorMessage = "Ingrese un monto")]
        public decimal dimmer;
        [Required(ErrorMessage = "Ingrese un monto")]
        public decimal seguidor;
        [Required(ErrorMessage = "Ingrese un monto")]
        public decimal precioJornada;
    }


    public class IngresosMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
        [Required(ErrorMessage = "Ingrese un número de factura")]
        public string factura;
        [Required(ErrorMessage = "Ingrese un tipo de factura")]
        public tiposFacturas tipoFactura;
        [Required(ErrorMessage = "Ingrese una fecha limite")]
        public System.DateTime fechaLimite;
        [Required(ErrorMessage = "Ingrese si es en blanco")]
        public bool enBlanco;       
        [Required(ErrorMessage = "Ingrese una fecha de emitida")]
        public System.DateTime fechaGenerada;       
        [Required(ErrorMessage = "Ingrese un valor")]        
        public decimal iva;
        [Required(ErrorMessage = "Ingrese un plan de cuenta")]
        public int idPlanCuentas;

    }


    public class LogCobranzasMetadata
    {
        [Required(ErrorMessage = "Ingrese una fecha")]
        public System.DateTime fecha;
    }


    public class LogEquiposMetadata
    {
        [Required(ErrorMessage = "Ingrese una fecha")]
        public System.DateTime fecha;
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
    }

    [Serializable]
    public class ModelosEquiposMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
        [Required(ErrorMessage = "Ingrese un costo por hora")]
        public decimal costoHora;
        [Required(ErrorMessage = "Ingrese un costo por día")]
        public decimal costoDia;
        [Required(ErrorMessage = "Ingrese un precio por hora")]
        public decimal precioHora;
        [Required(ErrorMessage = "Ingrese un precio por día")]
        public decimal precioDia;

        [JsonIgnore]
        public virtual ICollection<Equipos> Equipos { get; set; }
        [JsonIgnore]
        public virtual TipoServicios TipoServicios { get; set; }
        [JsonIgnore]
        public virtual ICollection<DetallePresupuesto> DetallePresupuesto { get; set; }
        [JsonIgnore]
        public virtual ICollection<MantenimientoPreventivo> MantenimientoPreventivo { get; set; }
    }


    public class MovimientosEquiposMetadata
    {
        [Required(ErrorMessage = "Ingrese un tipo de movimiento")]
        public int tipoMovimiento;
        [Required(ErrorMessage = "Ingrese una fecha")]
        public System.DateTime fecha;
        [Required(ErrorMessage = "Ingrese un código de barras")]
        public string codigoBarras;
        [Required(ErrorMessage = "Ingrese una cantidad")]
        [Range(1, 150, ErrorMessage = "La cantidad tiene que estar entre 1 y 150")]
        public int cantidad;
    }

    public partial class MovimientosEquipos
    {
        public int cantidad { get; set; }
    }


    public class PlanCuentasMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
    }
    
 


    public class PresupuestosMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
        [Required(ErrorMessage = "Ingrese una fecha de inicio")]
        public System.DateTime fechaInicio;
        [Required(ErrorMessage = "Ingrese una fecha de finalización")]
        [GreaterThan("fechaInicio", ErrorMessage = "La fecha de finalización debe ser posterior a la de inicio")]
        public System.DateTime fechaFin { get; set; }
        [Required(ErrorMessage = "Ingrese un estado")]
        public int estado;
    }



    public class SueldosMetadata
    {
        [Required(ErrorMessage = "Ingrese una base")]
        public decimal @base;
        [Required(ErrorMessage = "Ingrese un monto")]
        [Range(0.1, Double.MaxValue, ErrorMessage = "El valor tiene que ser superior a 0")]
        public decimal monto;
        [Required(ErrorMessage = "Ingrese un período")]
        public string periodo;
        [Required(ErrorMessage = "Ingrese una fecha")]
        public System.DateTime fecha;

    }




    public class TipoCuentasMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
    }

    [Serializable]
    public class TipoEgresosMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
       
        [Required(ErrorMessage = "Ingrese a qué esta vinculado")]
        public int vinculadoA;

        [JsonIgnore]
        public virtual ICollection<DetalleEgresos> DetalleEgresos { get; set; }
   
     
    }


    public class TipoIngresosMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;      
        [Required(ErrorMessage = "Ingrese a qué esta vinculado")]
        public int vinculadoA;
    }


    public class TipoServiciosMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
    }


    public class TipoTercerosMetadata
    {
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
    }


    public class TransferenciasMetadata
    {
        public int idCuentaOrigen { get; set; }
        [NotEqualTo("idCuentaOrigen", ErrorMessage = "La cuenta de destino debe ser distinta a la de origen")]
        public int idCuentaDestino { get; set; }
        [Required(ErrorMessage = "Ingrese una fecha")]
        public System.DateTime fecha;
        [Required(ErrorMessage = "Ingrese un monto")]
        [Range(0.1, Double.MaxValue, ErrorMessage = "El valor tiene que ser superior a 0")]
        public decimal monto;
        [Required(ErrorMessage = "Ingrese una descripción")]
        public string descripcion;
    }

}