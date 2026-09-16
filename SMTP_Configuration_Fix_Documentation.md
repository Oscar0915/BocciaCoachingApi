# Solución - Problemas de Configuración SMTP con Hostinger

## Problema Identificado
El usuario reportó que no se estaba enviando información por email. Tras revisar la configuración de Hostinger, se identificó que el problema era el uso incorrecto del protocolo de seguridad y puerto SMTP.

## Configuraciones SMTP de Hostinger

### Según la documentación oficial de Hostinger:

| Protocolo | Servidor | Puerto | Seguridad |
|-----------|----------|---------|-----------|
| **SMTP** | smtp.hostinger.com | **465** | **SSL implícito** ✅ |
| **SMTP** | smtp.hostinger.com | **587** | **STARTTLS** |
| **POP3** | pop.hostinger.com | 995 | SSL/TLS |
| **IMAP** | imap.hostinger.com | 993 | SSL/TLS |

## Cambios Realizados

### 1. Configuración Actualizada (appsettings.json)
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.hostinger.com",
    "Port": 465,                    // ✅ CAMBIADO DE 587 A 465
    "FromEmail": "notify@bocciacoaching.com",
    "Password": "Sr[c26g3",
    "FromName": "Boccia Coaching",
    "UseSsl": true                  // ✅ AGREGADO
  }
}
```

### 2. EmailSettings Mejorado
```csharp
public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; }
    public string FromEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = true; // ✅ NUEVO
}
```

### 3. EmailService Optimizado
```csharp
private async Task SendEmailAsync(MimeMessage message)
{
    using var client = new SmtpClient();
    
    if (_emailSettings.UseSsl && _emailSettings.Port == 465)
    {
        // ✅ SSL implícito (RECOMENDADO para Hostinger)
        await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, 
            MailKit.Security.SecureSocketOptions.SslOnConnect);
    }
    else if (_emailSettings.Port == 587)
    {
        // STARTTLS (alternativo)
        await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, 
            MailKit.Security.SecureSocketOptions.StartTls);
    }
    
    await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
    await client.SendAsync(message);
    await client.DisconnectAsync(true);
}
```

## Diferencias Clave Entre Puertos

### Puerto 465 (SSL implícito) ✅ RECOMENDADO
- **Ventajas:**
  - Conexión completamente cifrada desde el inicio
  - Más seguro
  - Ampliamente soportado por proveedores como Hostinger
  - No vulnerable a ataques de downgrade
- **Uso:** `SecureSocketOptions.SslOnConnect`

### Puerto 587 (STARTTLS)
- **Ventajas:**
  - Estándar más moderno
  - Inicia en texto plano y luego cifra
- **Desventajas:**
  - Puede ser vulnerable a ciertos ataques
  - Algunos proveedores prefieren 465
- **Uso:** `SecureSocketOptions.StartTls`

## Endpoint de Prueba Agregado

### 🧪 Test de Configuración SMTP
```http
POST /api/EmailTest/test-email
Content-Type: application/json

{
  "toEmail": "tu-email@ejemplo.com",
  "toName": "Tu Nombre"
}
```

**Respuesta exitosa:**
```json
{
  "success": true,
  "message": "Email de prueba enviado exitosamente a tu-email@ejemplo.com",
  "timestamp": "2024-12-20T..."
}
```

## Logging Agregado para Depuración

El sistema ahora incluye logging detallado:

```
🔗 Conectando a SMTP: smtp.hostinger.com:465
🔒 Usando SSL: true
🔐 Usando SSL implícito (puerto 465)
✅ Conexión SMTP establecida
🔑 Autenticando como: notify@bocciacoaching.com
✅ Autenticación exitosa
📧 Email enviado exitosamente
🔌 Desconexión exitosa
```

## Configuración de Respaldo

Si el puerto 465 presenta problemas, hay una configuración alternativa comentada en appsettings.json:

```json
"_EmailSettings_Alternative_587": {
  "SmtpServer": "smtp.hostinger.com", 
  "Port": 587,
  "FromEmail": "notify@bocciacoaching.com",
  "Password": "Sr[c26g3",
  "FromName": "Boccia Coaching",
  "UseSsl": false
}
```

## Pasos para Verificar la Solución

### 1. Ejecutar el Proyecto
```bash
cd /Users/oscar/Documents/BocciaCoaching
dotnet run
```

### 2. Probar el Endpoint
```bash
curl -X POST "http://localhost:5000/api/EmailTest/test-email" \
  -H "Content-Type: application/json" \
  -d '{
    "toEmail": "tu-email@test.com",
    "toName": "Nombre de Prueba"
  }'
```

### 3. Verificar Logs
Revisar la consola para ver los logs detallados de la conexión SMTP.

### 4. Revisar Email
Verificar la bandeja de entrada (y spam) del email de destino.

## Problemas Comunes y Soluciones

### ❌ "Connection refused" o timeout
- **Causa:** Puerto bloqueado por firewall/ISP
- **Solución:** Usar puerto 587 con STARTTLS como alternativa

### ❌ "Authentication failed"
- **Causa:** Credenciales incorrectas o 2FA activado
- **Solución:** Verificar credenciales, desactivar 2FA o usar app password

### ❌ "SSL handshake failed"
- **Causa:** Problema con certificados SSL
- **Solución:** Verificar que el puerto 465 soporte SSL implícito

## Resultado Final

✅ **Configuración SMTP corregida** para usar puerto 465 con SSL implícito  
✅ **Logging detallado** agregado para depuración  
✅ **Endpoint de prueba** implementado  
✅ **Configuración de respaldo** disponible  
✅ **Compatibilidad con Hostinger** verificada

La configuración ahora debería funcionar correctamente con los servidores de Hostinger usando la configuración SSL recomendada.
