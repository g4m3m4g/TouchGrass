# Stage 1: Build Angular
FROM node:24 AS build-step
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build -- --configuration=production

# Stage 2: Serve with Nginx
FROM nginx:alpine
# คัดลอกไฟล์ที่ build เสร็จแล้วจาก Angular
COPY --from=build-step /app/dist/frontend/browser /usr/share/nginx/html

# คัดลอกไฟล์คอนฟิกคัสตอมของ Nginx (เพื่อรองรับ Angular Routing ไม่ให้ขึ้น 404 เวลา Refresh หน้าเว็บ)
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]