-- 1. Crear la Base de Datos para el Microservicio de Pedidos
CREATE DATABASE IF NOT EXISTS orders_service_db;
USE orders_service_db;

-- 2. Tabla de Pedidos (Orders)
CREATE TABLE Orders (
    order_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    order_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    delivery_address VARCHAR(255) NOT NULL,
    total_amount DECIMAL(10, 2) NOT NULL,
    current_status ENUM('PENDIENTE', 'EN PROCESAMIENTO', 'ENVIADO', 'ENTREGADO', 'CANCELADO') 
        DEFAULT 'PENDIENTE' NOT NULL,
    tracking_number VARCHAR(50) NULL,
    cancellation_reason VARCHAR(255) NULL
);

-- 3. Tabla de Detalles del Pedido (Order_Items)
CREATE TABLE Order_Items (
    item_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    price_at_purchase DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE CASCADE
);

-- 4. Tabla de Historial de Estado del Pedido (Order_Status_History)
CREATE TABLE Order_Status_History (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    status_change_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    new_status ENUM('PENDIENTE', 'EN PROCESAMIENTO', 'ENVIADO', 'ENTREGADO', 'CANCELADO') NOT NULL,
    updated_by_user_id INT NULL,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE CASCADE
);