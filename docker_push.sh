#!/bin/bash
echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin
docker push michaelguenter/radio-frontend
docker push michaelguenter/radio-backend-external
docker push michaelguenter/radio-backend-internal
docker push michaelguenter/radio-backend-console
docker push michaelguenter/radio-streaming
docker push michaelguenter/radio-playback