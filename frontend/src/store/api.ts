import { apiBaseUrl } from '../config';

export function get(url: string): Promise<Response> {
    return fetch(apiBaseUrl + url, {
        method: "GET",
        credentials: "include"
    });
}

export function post(url: string, body?: string | Blob | ArrayBufferView | ArrayBuffer | FormData | URLSearchParams | ReadableStream<Uint8Array> | null): Promise<Response> {
    return fetch(apiBaseUrl + url, {
        method: "POST",
        credentials: "include",
        body: body
    });
}