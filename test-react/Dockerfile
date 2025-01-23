# =============================
# Stage 0: Install dependencies and run tests
# =============================
FROM node:18-alpine AS test
WORKDIR /app

# Copy package.json and package-lock.json
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application
COPY . .

# Run tests
RUN npm test

# =============================
# Stage 1: Build the React App
# =============================
FROM test AS build

# Build the React app for production
RUN npm run build

# =============================
# Stage 2: Serve with NGINX
# =============================
FROM nginx:stable-alpine AS serve

# Copy the built files from the build stage
COPY --from=build /app/dist /usr/share/nginx/html

# Copy the custom NGINX configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose the default NGINX port
EXPOSE 80

# Start NGINX server
CMD ["nginx", "-g", "daemon off;"]
