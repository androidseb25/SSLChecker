# SSLChecker

SSLChecker is a small Docker container that checks your websites for a valid SSL certificate. If the expiration date is approaching, the container notifies you via the Gotify integration.
&nbsp;
## ⭐ Features

* gotify integration
* checks multiple websites
* interval & time scheduling
* notification in 30, 20, 10, 5, 3, 2, 1 days

&nbsp;
## 🔧 How to Install SSLChecker

### 🐳 Docker `docker-compose.yaml`

### Installation

1. Create a file with the name `docker-compose.yaml` or clone this repo and go to step 3
2. Please use the latest and recommended version of docker and docker compose
3. Copy the code down below in the yaml file
4. change environment variables in the compose file
5. execute `docker compose up -d` to start the docker compose

&nbsp;
### Needed environment variables

* `DOMAINS` = the list of websites that checks seperated by ';'

### Optional environment variables

* `GOTIFY_URL` = you're Gotify Url to send an information e.g. `https://my-gotify.com`
* `GOTIFY_APP_TOKEN` = the application token from gotify e.g. `ATjChbj3_XTc9P.`
* `GOTIFY_PRIORITY` = default value is `0`
* `SEND_MESSAGE_ON_SUCCESS` = send a message when certificate is still valid. default: `false`
* `INTERVAL` =  when you run the check in hour's run. default: `24`
* `START_TIME` = when the job is starting to check. default: `03:00`


&nbsp;

```bash
version: '3.8'

services:
  sslchecker:
    container_name: SSLChecker
    hostname: sslchecker
    image: ghcr.io/androidseb25/sslchecker:latest
    restart: unless-stopped
    security_opt:
      - no-new-privileges:true
    pull_policy: always
    networks:
      - net
    ports:
      - "8454:8080"
    volumes:
      - data:/app/data
    environment:
      DOMAINS: '' #domain list without http/s e.g. 'example.com' more domains seperated by ';'
      #GOTIFY_URL: '' #OPTIONAL: you're Gotify Url to send an information e.g. 'https://my-gotify.com'
      #GOTIFY_APP_TOKEN: '' #OPTIONAL: the application token from gotify e.g. 'ATjChbj3_XTc9P.'
      #GOTIFY_PRIORITY: '' # OPTIONAL: default value is '0' 
      #SEND_MESSAGE_ON_SUCCESS: '' # OPTIONAL: send a message when certificate is still valid. the default value is 'false' 
      #INTERVAL: '' # OPTIONAL: when you run the check in hour's run. the default value is '24' 
      #START_TIME: '' # OPTIONAL: when the job is starting to check. the default value is '03:00' 

networks:
  net:

volumes:
  data:
```
