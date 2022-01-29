# NetworkDashboard
Network Monitoring Solution built with C#.

## Polling
Polling is completed with a worker service that runs on a remote server. This worker service checks status of the remote devices through ping, using hostname and IP Address, or a HTTP navigation check. 
Once the status is received, it is updated in the MSSQL database. 

## Dashboard
The dashboard displays the configured device status at the index. The index is sorted into the different device polling types, called Monitor Types, that are configured.

### Polling Settings:
  The poller settings are stored in the database. Currently this is only the polling interval. This interval is configured in the settings section within the dashboard. 
  After the current iteration the poller will check the database for updated poll settings.
  

#Screenshots:

![Index](https://github.com/LeviBickel/NetworkDashboard/blob/master/image.jpg?raw=true)
![Monitor Types](https://github.com/LeviBickel/NetworkDashboard/blob/master/image.jpg?raw=true)

