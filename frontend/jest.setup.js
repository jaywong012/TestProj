import { TextEncoder, TextDecoder } from 'util';

// Polyfill TextEncoder if not available
if (typeof global.TextEncoder === 'undefined') {
  global.TextEncoder = TextEncoder;
}

// Polyfill TextDecoder if not available
if (typeof global.TextDecoder === 'undefined') {
  global.TextDecoder = TextDecoder;
}