INSERT INTO notes(title, content, details)
VALUES
('Docker Run','Run a container from an image', 'docker run <image name>'),
('Docker Build','Build an image from a docker file', 'docker build -t myapp:latest .'),
('Docker Pull','Pull an image from a registry', 'docker pull <image name>'),
('Docker Push', 'Push an image to a registry','docker push myuser/myapp:latest'),
('Docker Images', 'List all available images on your machine','docker images')