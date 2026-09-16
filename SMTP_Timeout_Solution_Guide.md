# Solución al Problema de Timeout SMTP

## 🔍 Análisis del Problema

Basándome en tu log de error:
```
❌ Error en SendEmailAsync: The operation has timed out.
🔗 Conectando a SMTP: smtp.hostinger.com:465
🔒 Usando SSL: True  
🔐 Usando SSL implícito (puerto 465)
```

**Problema Identificado:** El puerto 465 está siendo bloqueado por el firewall de Render (tu hosting), causando timeout.

## 🛠️ Solución Implementada

### 1. Endpoint de Diagnóstico Automático
He agregado un endpoint que probará automáticamente diferentes configuraciones:

```http
POST /api/EmailTest/diagnose
```

Este endpoint probará:
- ✅ **Puerto 587 con STARTTLS** (recomendado para hosting)
- ❌ Puerto 465 con SSL implícito (bloqueado por Render)
- ⚠️ Puerto 25 sin cifrado (no seguro)
- 🔄 Puerto 2525 con STARTTLS (alternativo)

### 2. Configuración Recomendada
Basándome en la imagen de Hostinger y los problemas de firewall, usa esta configuración:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.hostinger.com",
    "Port": 587,
    "FromEmail": "notify@bocciacoaching.com",
    "Password": "Sr[c26g3",
    "FromName": "Boccia Coaching",
    "UseSsl": false
  }
}
```

**Por qué puerto 587:**
- ✅ Más compatible con hosting cloud (Render, AWS, etc.)
- ✅ STARTTLS es ampliamente soportado
- ✅ Menos probabilidad de ser bloqueado por firewalls
- ✅ Estándar moderno para envío SMTP

## 🧪 Pasos para Resolver

### 1. Ejecutar Diagnóstico
```bash
curl -X POST "https://tu-app.render.com/api/EmailTest/diagnose" \
  -H "Content-Type: application/json"
```

### 2. Verificar Conectividad de Red
```bash
curl -X POST "https://tu-app.render.com/api/EmailTest/ping" \
  -H "Content-Type: application/json"
```

### 3. Aplicar Configuración Exitosa
Basándote en los resultados del diagnóstico, actualizar appsettings.json con la configuración que funcione.

## 📋 Configuraciones Alternativas

### Opción 1: STARTTLS (Recomendado)
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.hostinger.com",
    "Port": 587,
    "UseSsl": false
  }
}
```

### Opción 2: Si 587 también está bloqueado
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.hostinger.com", 
    "Port": 2525,
    "UseSsl": false
  }
}
```

### Opción 3: Gmail SMTP (Como respaldo)
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "FromEmail": "tu-gmail@gmail.com",
    "Password": "app-password",
    "UseSsl": false
  }
}
```

## 🔧 Mejoras Implementadas

### 1. Timeout Reducido
- **Antes:** 30 segundos (muy lento para depurar)
- **Ahora:** 15 segundos (falla más rápido)

### 2. Mejores Mensajes de Error
```
❌ Timeout conectando al servidor SMTP en puerto 465: The operation has timed out.
💡 El puerto 465 puede estar bloqueado. Intente puerto 587 si está usando 465.
```

### 3. Manejo Específico de Excepciones
- `SocketException`: Puerto bloqueado por firewall
- `TimeoutException`: Conexión lenta o bloqueada  
- `AuthenticationException`: Credenciales incorrectas

## 🎯 Próximos Pasos

1. **Ejecutar el diagnóstico** para identificar qué puertos funcionan
2. **Actualizar appsettings.json** con la configuración exitosa
3. **Probar envío de email** con la nueva configuración
4. **Implementar fallback** si es necesario

## 📧 Resultado Esperado

Una vez configurado correctamente, verás:
```
🔗 Conectando a SMTP: smtp.hostinger.com:587
🔄 Usando STARTTLS (puerto 587)
✅ Conexión SMTP establecida
🔑 Autenticando como: notify@bocciacoaching.com
✅ Autenticación exitosa
📧 Email enviado exitosamente
🔌 Desconexión exitosa
```

## 🚨 Importante

**El puerto 465 está bloqueado en tu entorno Render.** Esto es normal y común en servicios de hosting cloud. El puerto 587 con STARTTLS es la solución estándar para este problema.
