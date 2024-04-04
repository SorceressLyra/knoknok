# Knoknok
> For bothering your beloved on the network!

Knoknok is a simple tray application that allows you to broadcast a knock on the network for others with the client to receive


### Current Features
- Sending a knock to all client on the network
- Receiving a knock acknowledge from receiver of knock

### Wanted future features
- Targeted knocks
- Possible text replies
- Simple mobile client
- Multiplatform support (currently only windows)
- Customizable port

### Information
The client by default uses port `11000` for communication with UDP, this will be customizable in the future in case you experience port conflicts.

The data sent between clients is a 2 byte package for now, this might change in the future:

| byte #1 | byte #2|
| ------------- | ------------- |
| Command code | User ID |


The various commands so far, these **WILL** be subject to change over time.
| Byte  | Purpose |
| ------------- | ------------- |
| 1 | Knock acknowledge  |
| 255 | Knock send |

![kirby](https://github.com/SorceressLyra/knoknok/assets/20424962/4689e617-5750-4abf-aafa-b085b90487cd)
