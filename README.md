## Cambios realizados Parte 1

- Elimine el script de **Start Session Manager** del **NetworkManager** que se usaba para crear los botones de **Host,Server y Client** y cree un GameObject de **Canvas** con una estructura igual que el usado en **NetTanks** con sus Buttons para cada una de las tres opciones y creé el script de **Connection Buttons** y se los asigne a los Botones para gestionar la conexión a traves de cada uno de ellos.

## Cambios realizados Parte 2

- Modifique el prefab de Player primero cambiando su autoridad de **Server a Owner** igual que en **NetTanks** para que el jugador tenga el control del player ademas de evitar posibles lags que puedan surgir en **Server**.

- Tambie modifique el script de **PlayerMovement** de manera que quede separado la lectura de los Input y la aplicación del movimiento.
- En **Update()** se captura la entrada de los Input para ello la variable de Vector2 pasa a declararse a nivel de clase para poder ser usada en **Update y FixedUpdate**.
- En el método **FixedUpdate()** se usan dos variables de Vector3, forward y right para calcular el movimiento del Player en función de su orientación, tabmien se elimina el componente vertical de ambos vectores para controlar que el player solo se mueva horizontalmente y evitar que pueda moverse hacia arriba o abajo según la orientación de la camara.

 - Luego se aplica el moviemento basado en los input del jugador , así como la orientación de la camara y la velocidad predefinida
 -  Por ultimo se mueve el RigidBody usando el **MovePosition**.

```csharp
 void FixedUpdate()
    {
        if (!IsOwner) return;

        // Dirección del movimiento basada en la orientación de la cámara
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // Se elimina el componente vertical para que el movimiento sea horizontal
        // y evitar salga disparado al mirar hacia arriba o abajo
        forward.y = 0;
        right.y = 0;

        // Se normalizan los vectores para mantener la dirección correcta sin afectar la velocidad
        forward.Normalize();
        right.Normalize();

        // Movimiento basado en el input del jugador, la orientación de la cámara y la velocidad
        Vector3 move = (forward * moveInput.y + right * moveInput.x) * speed * Time.fixedDeltaTime;

        // Mueve el Rigidbody usando MovePosition para mantener una correcta interacción con las físicas
        rb.MovePosition(rb.position + move);
    }
```

- Como se cambio la Autoridad a Owner elimine el **[Rpc(SendTo.Server)]** del script ya que **ahora es el Jugador el que controla al Player y no el servidor**.
  
- Tambien elimine el metodo **TranslateRPC()** ya que no hace falta ya que ahora el movimiento se aplica en el **FixedUpdate()**

## NETCrazyZombie multijugador

### Mejoras sugeridas:

- Crear el sistema de proyectiles de igual forma que se hace en NETTanks.
  - Prefab base
  - Prefab cliente
  - Prefab servidor

- Separar el sistema de salud del jugador de forma similar a NETTanks.

- Separar el sistema del display de salud de forma similar a NETTanks.

- Implementar un sistema de respawn que funcione correctamente.

### Mejoras sugeridas (investigación, avanzado):

- Cambiar el sistema de cámaras, utilizando el paquete Cinemachine. Se utiliza en NETTanks en la rama online.

### Bugs detectados:

- Los zombies se mueven erráticamente en ocasiones cuando chocan con las escaleras y no pueden alcanzar al jugador.
