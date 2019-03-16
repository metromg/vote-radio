FROM node:8 as build-deps

WORKDIR /usr/src/app

COPY package*.json ./
RUN npm install

COPY . ./

RUN npm run build
RUN npm prune --production

FROM node:8

ENV NODE_ENV=production

WORKDIR /usr/src/app

COPY --from=build-deps /usr/src/app/dist ./
COPY --from=build-deps /usr/src/app/node_modules ./node_modules/

USER node

EXPOSE 8080
VOLUME ["/home/node/watch"]

CMD ["node", "index.js"]