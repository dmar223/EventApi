# EventApi

A minimal event ingestion API built using ASP.NET Core and .NET 10.

The service accepts JSON events via HTTP, validates requests, and asynchronously queues events for background processing.

## Features

- `POST /events` ingestion endpoint
- Request validation
- API key authentication
- Asynchronous event publishing
- In-memory queue abstraction using `Channel<T>`
- Background worker processing
- Structured logging
- Unit tests for core behaviours

---

# Architecture Overview

The service is intentionally designed with clear separation between ingestion and processing.

```text
Client
  ↓
EventsController
  ↓
Validation
  ↓
EventPublisher
  ↓
Message Queue Abstraction
  ↓
In-Memory Channel Queue
  ↓
Event Processor Job
```

# Running the Service

Run the API locally with:

```bash
dotnet run
```

## Authentication

The API uses a simple API key mechanism for demonstration purposes.

Clients must provide:

```text
X-API-Key: <key>
```

The API key is configured in:

```text
appsettings.json
```

Example:

```json
{
  "ApiKey": "dev-test-key"
}
```

---

## Example Request

```http
POST /events
Content-Type: application/json
X-API-Key: dev-test-key
```

```json
{
  "event_id": "123",
  "source": "example-client",
  "timestamp": "2026-05-16T18:00:00Z",
  "payload": {
    "message": "Hello world"
  }
}
```

---

# Response Behaviour

## Successful Request

```http
202 Accepted
```

Returned after:
- authentication
- validation
- successful enqueue into the message queue

The API acknowledges receipt by the ingestion service, not completion of downstream processing.

## Validation Failure

```http
400 Bad Request
```

Returned for:
- missing required fields
- invalid timestamp format
- malformed requests

## Queue Failure

```http
503 Service Unavailable
```

Returned if the event cannot be accepted into the queue.

---

# Validation Approach

Basic request validation is handled using ASP.NET model binding and data annotations.

Additional validation logic, such as timestamp parsing, is handled explicitly within the application. This keeps the validation logic flexible and independently testable.

This provides flexibility in how validation is handled, but does mean validation is split between data annotations and a validation service. I chose not to rely on Regex-based ISO-8601 validation because date parsing logic is complex, and parsing directly with `DateTimeOffset` is more maintainable and reliable.

---

# Background Processing

Events are processed asynchronously using a hosted background service.

Current processing behaviour is intentionally simple and logs received events.

Processing failures are isolated from ingestion and do not prevent the API from accepting new requests.

---

# Security Considerations

This implementation intentionally uses a lightweight authentication approach suitable for the assignment.

Production considerations would include:

- Secret management
- Key rotation
- HTTPS enforcement
- Rate limiting
- Replay protection
- Centralised authentication/authorisation
- Durable messaging infrastructure
- Monitoring and alerting

---

# Trade-offs

## In-Memory Queue

An in-memory channel was chosen to keep the implementation lightweight while still demonstrating asynchronous decoupling.

This does not provide:
- durability
- persistence across restarts
- distributed scaling

In production, this abstraction would likely be replaced with a durable external broker which would also allow the processing side to be handled by a completely separate service.

## Lightweight Validation Responses

Validation responses are intentionally simple to keep focus on service structure and behaviour rather than API contract standardisation. Depending on the API exposure and intended consumers, limiting validation detail may also reduce unnecessary information disclosure.

---

# Future Improvements

Potential future enhancements include:

- Durable message broker integration
- Structured error contracts
- Retry policies
- Dead-letter queue support
- Metrics and tracing
- Rate limiting
- Persistent event storage
- Improved validation contracts
- Integration testing