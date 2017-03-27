using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication
{
    [MetadataType(typeof(AdelantosMetadata))]
    public partial class Adelantos { }

    [MetadataType(typeof(MantenimientoPreventivoMetadata))]
    public partial class MantenimientoPreventivo { }

    [MetadataType(typeof(DetalleMantenimientoPreventivoMetadata))]
    public partial class DetalleMantenimientoPreventivo { }
    
    [MetadataType(typeof(ValesMetadata))]
    public partial class Vales { }

    [MetadataType(typeof(ProximasComprasMetadata))]
    public partial class ProximasCompras { }

    [MetadataType(typeof(PagosMetadata))]
    public partial class Pagos { }

    [MetadataType(typeof(CobrosMetadata))]
    public partial class Cobros { }

    [MetadataType(typeof(ChequesMetadata))]
    public partial class Cheques { }


    [MetadataType(typeof(ChequesPorEgresoMetadata))]
    public partial class ChequesPorEgreso { }


    [MetadataType(typeof(ChequesPorIngresoMetadata))]
    public partial class ChequesPorIngreso { }


    [MetadataType(typeof(CuentasMetadata))]
    public partial class Cuentas { }


    [MetadataType(typeof(DetalleEgresosMetadata))]
    public partial class DetalleEgresos { }


    [MetadataType(typeof(DetalleIngresosMetadata))]
    public partial class DetalleIngresos { }


    [MetadataType(typeof(DetallePresupuestoMetadata))]
    public partial class DetallePresupuesto { }


    [MetadataType(typeof(EgresosMetadata))]
    public partial class Egresos { }


    [MetadataType(typeof(EquiposMetadata))]
    public partial class Equipos { }


    [MetadataType(typeof(EventosMetadata))]
    public partial class Eventos { }


    [MetadataType(typeof(HorasNoTrabajadasMetadata))]
    public partial class HorasNoTrabajadas { }


    [MetadataType(typeof(HorasTrabajadasTecnicoMetadata))]
    public partial class HorasTrabajadasTecnico { }


    [MetadataType(typeof(IngresosMetadata))]
    public partial class Ingresos { }


    [MetadataType(typeof(LogCobranzasMetadata))]
    public partial class LogCobranzas { }


    [MetadataType(typeof(LogEquiposMetadata))]
    public partial class LogEquipos { }


    [MetadataType(typeof(ModelosEquiposMetadata))]
    public partial class ModelosEquipos { }


    [MetadataType(typeof(MovimientosEquiposMetadata))]
    public partial class MovimientosEquipos { }


    [MetadataType(typeof(PlanCuentasMetadata))]
    public partial class PlanCuentas { }


   
    [MetadataType(typeof(PresupuestosMetadata))]
    public partial class Presupuestos { }


    [MetadataType(typeof(SueldosMetadata))]
    public partial class Sueldos { }


    [MetadataType(typeof(TercerosMetadata))]
    public partial class Terceros { }


    [MetadataType(typeof(TipoCuentasMetadata))]
    public partial class TipoCuentas { }


    [MetadataType(typeof(TipoEgresosMetadata))]
    public partial class TipoEgresos { }


    [MetadataType(typeof(TipoIngresosMetadata))]
    public partial class TipoIngresos { }


    [MetadataType(typeof(TipoServiciosMetadata))]
    public partial class TipoServicios { }


    [MetadataType(typeof(TipoTercerosMetadata))]
    public partial class TipoTerceros { }


    [MetadataType(typeof(TransferenciasMetadata))]
    public partial class Transferencias { }



}