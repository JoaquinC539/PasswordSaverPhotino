type RequestResolver = (response: any) => void;

export class MessageBus {
  private requestId = 0;
  private pending = new Map<number, RequestResolver>();
  private listeners: ((msg: any) => void)[] = [];

  constructor() {
    window.external.receiveMessage((raw) => {
      const msg = JSON.parse(raw);

      // If it's a response to a request
      if (msg.id && this.pending.has(msg.id)) {
        const resolver = this.pending.get(msg.id)!;
        this.pending.delete(msg.id);
        resolver(msg.payload);
      }

      // Broadcast to all listeners
      this.listeners.forEach((fn) => fn(msg));
    });
  }

  // Fire-and-forget
  public send(type: string, payload?: any) {
    const msg = { type, payload };
    window.external.sendMessage(JSON.stringify(msg));
  }

  // Request/response style (like Electron invoke)
  public request<T = any>(type: string, payload?: any): Promise<T> {
    const id = ++this.requestId;
    const msg = { id, type, payload };
    window.external.sendMessage(JSON.stringify(msg));

    return new Promise<T>((resolve) => {
      this.pending.set(id, resolve);
    });
  }

  // Subscribe to all messages (event-style)
  public subscribe(fn: (msg: any) => void) {
    this.listeners.push(fn);
  }
}
