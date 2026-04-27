## Cambios realizados Parte 1

- Elimine el script de **Start Session Manager** del **NetworkManager** que se usaba para crear los botones de **Host,Server y Client** y cree un GameObject de **Canvas** con una estructura igual que el usado en **NetTanks** con sus Buttons para cada una de las tres opciones y creé el script de **Connection Buttons** y se los asigne a los Botones para gestionar la conexión a traves de cada uno de ellos.


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
