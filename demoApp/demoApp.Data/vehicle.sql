  CREATE TABLE vehicle (
  id SERIAL PRIMARY KEY,  
  licensePlate VARCHAR(100),
  temperauture DOUBLE PRECISION,
  lat DOUBLE PRECISION,
  lon DOUBLE PRECISION
  );

CREATE TABLE ride (
  id SERIAL PRIMARY KEY, 
  vehicleid int NOT NULL,
  licensePlate VARCHAR(100),
  ts TIMESTAMPTZ DEFAULT NOW(),
  lat DOUBLE PRECISION,
  lon DOUBLE PRECISION,
  createdat TIMESTAMPTZ DEFAULT NOW(),
  CONSTRAINT fk_vehicle FOREIGN KEY(vehicleid) REFERENCES vehicle(id)
  );
