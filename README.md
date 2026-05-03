## Cambios realizados

- A partir del Bullet crée dos copias del Prefab **Bullet Server** que es la bala "real" que aplica daño, detecta las colisiones y se sincroniza entre los clientes y la **Bullet Client** que basicamente es la bala visual que podran visualizar los jugadores en la partida.
- Luego separe todas las responsabilidades en distintos scripts:
  - **Bullet Move** : Paso de ser un ***NetworkBehaviour a un Monobehaviour*** esto debido a que este script esta en Bullet Client el cual deja de ser un Network Object a diferencia del Server , y eso causaba conflicto con el prefab, realmente no es necesario que sea un networkBehaviour ya que ahora solo se usa basicamente para mover la Bala no como antes que basicamente hacia todo.
  - **DealDamageContact** : Este script ahora registra y aplica el daño que reciben los Players al ser disparados.
  - **BulletHit** : Activa los efectos visuales y se ocupa de la destrucción del proyectil.
  - **LifeTime** : En este script basicamente la bala se destruye despues de unos segundos despues de ser disparada en caso de que no impacte contra ningúun objetivo y obstaculo.
  - **Player Fire Bullet** : Creé un emptyObject **ShootPoint** dentro del GameObject Weapon del Prefab **Player** , un poco separado para corregir un pequeño error que podia hacer que el player se dañase a si mismo al disparar, en el Script de **PlayerFireBullet** se instancia la Bala que podran visualizar los distintos jugadores en pantalla.
  - **PlayerManager** : Fue simplificado , se elimino el sistema de vida el cual fue trasladado a **HealthPlayer**, tambien el sistema de daño y destrucción que ahora se hacen en **BulletHit y DealDamageContact**, el PlayerManager no gestiona ya las colisiones si no los objetos que lo causan, se separo la visualización de vida por **HealthDisplayPlayer**, lo que si sigue haciendo es escuchar y actualizar el texto que indica cuanta vida le queda a cada jugador, ahora el spawn es controlado por el servidor.
    
  - **HealtPlayer** : Este script ahora se ocupa de gestionar la vida del jugador, recibir daño su muerte y los eventos de vida, sigue siendo una NetworkVariable.
  - **HealthDisplayPlayer** :  Es el script que se usa para gestionar la UI de la barra de vida de los player, el cual esta asigando al GameObject de PlayerDataUI que esta en el prefab del player.

- **Spawn y Respawn** : Se crearon dentro de un EmptyGameObject llamado PlayerSpawnerPoints dentro de cual hay predefinidos tres puntos de spawn luego converti este gameObject en un Prefab y modifique el script **SpawnPointManager** para que pueda escoger aleatoriamente entre los puntos de spawn ya definidos y que cuando un jugador se una no pueda usar aquel punto que ya haya sido usado al  momento de unirse por otro jugador.




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
