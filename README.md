# 🛒 Sistema Integral de Gestión – Supermercado NovaMarket

## 📌 Proyecto
Sistema desarrollado para mejorar el control operativo, financiero y estratégico de un supermercado en crecimiento.

El objetivo principal es dejar de manejar el negocio "a ojo" y contar con información clara, trazabilidad de movimientos y control real del inventario y las ganancias.

---

## 🗣️ Entrevista – Dueño del Supermercado NovaMarket

Mira ingeniero, te voy a hablar claro. El supermercado está creciendo, vendemos más que antes, pero siento que estoy manejando todo con alambre. Yo soy el administrador, pero también termino apagando incendios todo el tiempo.

El problema más grande lo tengo con los productos. El sistema actual —si es que se le puede llamar sistema— no me da confianza. A veces me dice que hay 50 unidades en stock, pero cuando voy al depósito hay menos. O al revés, aparece que no hay producto y en realidad sí hay. Entonces no sé si es que alguien vendió sin registrar, si cargaron mal la mercadería o si alguien modificó algo que no debía.

Y ahí está otro punto delicado… cualquiera de mis empleados puede entrar y cambiar cantidades en el inventario. Yo confío en mi gente, pero el negocio no puede funcionar solo con confianza. Si alguien modifica el stock, yo necesito saber quién fue, a qué hora y qué cambió exactamente. No para retarlos, sino porque si hay pérdidas, necesito entender dónde se generan.

Con las compras de los clientes también estoy medio perdido. Vendemos bastante, eso lo veo en la caja, pero no tengo claridad de cuáles productos realmente me dejan margen. Hay productos que rotan mucho, pero no sé si son los más rentables o solo los más baratos. A veces hago descuentos “porque creo” que conviene, pero no tengo un reporte que me diga si esa promoción me hizo ganar o perder dinero.

Y hablando de dinero… ese es otro dolor de cabeza. Yo veo cuánto entra en efectivo y transferencias, pero no sé exactamente cuánto estoy ganando al mes. Porque vender no es lo mismo que ganar. Tengo que pagar proveedores, sueldos, servicios, impuestos… y también hay productos que se vencen o se dañan. Yo necesito algo que me diga claramente: “Este mes tu utilidad neta fue tanto”. Sin tener que hacer cuentas manuales en una hoja aparte.

También quiero organizar mejor los roles. Yo como administrador debería tener acceso total: ver reportes, modificar precios, eliminar productos si es necesario. El empleado debería poder vender y registrar ingreso de mercadería, pero no debería poder cambiar precios libremente o borrar movimientos. Y el usuario… bueno, estoy pensando que en el futuro podríamos tener clientes registrados, que puedan consultar sus compras o acumular puntos. Pero obviamente no quiero que tengan acceso a información interna del negocio.

Otro tema que me preocupa son los errores en caja. Cuando falta dinero o hay una devolución rara, nadie sabe qué pasó. Si cada venta estuviera asociada al empleado que la hizo, yo podría revisar con más tranquilidad. No quiero estar desconfiando de todos, pero necesito control.

Además, quiero información más estratégica. Algo que me diga cuáles son los productos más vendidos, cuáles casi no se mueven, qué empleado vende más, en qué horarios hay más movimiento. No quiero algo demasiado complejo, pero sí algo que me dé datos claros para tomar decisiones.

En resumen ingeniero, lo que yo necesito es:

Control real del inventario.

Saber quién hace cada movimiento.

Diferenciar bien los roles (administrador, empleado y usuario).

Tener reportes claros de ventas y ganancias.

Y dejar de manejar el negocio “a ojo”.

Yo no hablo mucho en términos técnicos, eso te lo dejo a ti. Pero si el sistema me da orden, claridad y números reales, ya es una gran mejora para el supermercado.

---

# 🚀 Metodología

- Metodología: Scrum
- Fase actual: Sprint 0 – Análisis y Formulación de Requerimientos
- Gestión del Backlog: GitHub Issues
- Control de versiones: Git + GitHub

---

# 📋 Sprint 0 – Backlog Inicial

## 🔴 Historias de Usuario – Alta Prioridad

---

### HU-01 – Gestión de Roles y Permisos

**Como** administrador  
**Quiero** que el sistema gestione roles diferenciados (Administrador, Empleado y Usuario)  
**Para** controlar el acceso a la información y evitar modificaciones no autorizadas.

**Criterios de aceptación:**
- El sistema permite crear usuarios con rol asignado.
- El Administrador tiene acceso total.
- El Empleado puede registrar ventas e ingreso de mercadería.
- El Usuario solo puede consultar su historial.
- El sistema bloquea modificaciones no permitidas según el rol.

---

### HU-02 – Control de Movimientos de Stock con Trazabilidad

**Como** administrador  
**Quiero** que cada modificación del inventario registre automáticamente el usuario, fecha, hora y tipo de movimiento  
**Para** tener control y trazabilidad ante errores o pérdidas.

**Criterios de aceptación:**
- Cada movimiento se guarda en un historial.
- Se registra producto, cantidad anterior y nueva.
- Se guarda usuario, fecha y hora.
- El historial no puede modificarse.
- El administrador puede visualizar el historial completo.

---

### HU-03 – Registro de Ventas

**Como** empleado  
**Quiero** registrar ventas de forma rápida y segura  
**Para** actualizar automáticamente el stock y garantizar el cobro correcto.

**Criterios de aceptación:**
- Permite agregar múltiples productos por venta.
- El stock disminuye automáticamente.
- La venta queda asociada al empleado.
- Se genera registro/comprobante de venta.

---

### HU-04 – Reporte de Ganancias Mensuales

**Como** administrador  
**Quiero** generar un reporte mensual detallado de ingresos, egresos y utilidad neta  
**Para** conocer la rentabilidad real del negocio.

**Criterios de aceptación:**
- Calcula ingresos totales del mes.
- Registra egresos (proveedores, sueldos, servicios).
- Calcula utilidad neta automáticamente.
- Permite seleccionar mes a consultar.

---

## 🟡 Historias de Usuario – Prioridad Media

---

### HU-05 – Reporte de Productos Más y Menos Vendidos

**Como** administrador  
**Quiero** visualizar productos más vendidos y menos vendidos  
**Para** tomar decisiones estratégicas sobre promociones y reposición.

**Criterios de aceptación:**
- Ordena productos por cantidad vendida.
- Permite filtrar por fechas.
- Muestra cantidad vendida y total generado.

---

### HU-06 – Control de Caja por Empleado

**Como** administrador  
**Quiero** que cada venta esté asociada al empleado que la realizó  
**Para** auditar diferencias o errores en caja.

**Criterios de aceptación:**
- Cada venta guarda ID del empleado.
- Se puede filtrar ventas por empleado.
- Se puede generar reporte por empleado.

---

## 🔵 Historias de Usuario – Prioridad Baja

---

### HU-07 – Historial de Compras del Cliente

**Como** usuario  
**Quiero** consultar mi historial de compras  
**Para** llevar control de mis gastos.

**Criterios de aceptación:**
- El usuario debe iniciar sesión.
- Solo puede ver sus propias compras.
- Se muestra fecha, productos y total pagado.

---

# 📊 Priorización del Backlog

| Prioridad | Historia |
|-----------|----------|
| 🔴 Alta | HU-01 Gestión de Roles |
| 🔴 Alta | HU-02 Control de Stock |
| 🔴 Alta | HU-03 Registro de Ventas |
| 🔴 Alta | HU-04 Reporte de Ganancias |
| 🟡 Media | HU-05 Reporte de Productos |
| 🟡 Media | HU-06 Control de Caja |
| 🔵 Baja | HU-07 Historial del Cliente |

---

# 🎯 Objetivo del Sprint 1

Implementar las funcionalidades críticas:
- Gestión de Roles
- Registro de Ventas
- Control de Inventario con trazabilidad
- Base del reporte financiero

---

# 📌 Resultado Esperado

Un sistema que proporcione:
- Control real del inventario
- Trazabilidad de movimientos
- Diferenciación clara de roles
- Reportes financieros confiables
- Información estratégica para la toma de decisiones
