FROM node:8 AS build

WORKDIR /usr/src/app
COPY . ./

RUN npm ci
RUN npm run build

RUN wget -qO mo https://git.io/get-mo && \
    chmod +x mo

FROM build AS testrunner
CMD ["npm", "test", "--no-watch"]

FROM nginx:latest

COPY --from=build /usr/src/app/build /usr/share/nginx/html
COPY --from=build /usr/src/app/mo /mo
COPY ./docker-entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]