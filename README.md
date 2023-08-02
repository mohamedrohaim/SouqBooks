# SouqBooks - Ecommerce Website Readme

## Overview
SouqBooks is an Ecommerce website built using ASP.NET MVC 5. It offers various features such as user management with authentication and authorization, roles, CRUD operations for products with categories, order management with different order statuses (pending, processing, shipped out for delivery, cancelled), and payment integration using Stripe. The website primarily targets the online selling of books but can be extended for other products as well.


### Features <a name="features"></a>

#### User Registration and Login with Authentication using Identity
- The website offers user registration and login functionality with secure authentication.
- New users can sign up with their email and password to create an account.
- The password is encrypted and requires a strong combination of characters, including at least one uppercase letter, one lowercase letter, and special characters.

#### User Roles for Different Access Levels
- Users are assigned different roles, such as "user" and "admin," to control their access to specific features and areas of the website.
- Admins have access to additional features, like managing user roles.\
![User Roles](https://i.postimg.cc/xCdgWwJK/Manage-user-Roles.png)

#### Product Browsing with Categories
- Users can browse through a wide range of products, categorized for easy navigation.
- Each category displays relevant products to help users find what they are looking for.
![Product Browsing](https://i.postimg.cc/NMTZ0ykg/user-Role-Home.png)


#### Product Details and Images Display
- Product pages provide comprehensive details about each item, including descriptions, prices, and images.
- Users can view product images to get a better understanding of the item before making a purchase.

#### Add Products to the Shopping Cart
- Users can add products to their shopping cart while browsing.
- The shopping cart displays a summary of selected items and their quantities.

#### Checkout and Order Placement
- Users can proceed to checkout after adding items to their cart.
- During checkout, users can review their order and provide necessary information, such as shipping address and payment details.

#### Order Status Tracking
- Users can track the status of their orders through the website.
- Order statuses include "Pending," "Processing," "Shipped out for Delivery," and "Cancelled."

#### Admin Dashboard
- The admin dashboard provides an overview of all orders and products.
- Admins can manage orders, update order statuses, and view order details.
![Admin Dashboard](https://i.postimg.cc/59JZkTyR/Admin-Dashboard.png)

#### Data Validation
- Input validation is performed on various forms and fields, such as user registration, login, product creation, and order placement.
- Appropriate error messages are displayed to users in case of invalid or missing data.
![Admin Dashboard](https://i.postimg.cc/vGQMq1kv/Product-With-Validation.png)

#### Payment Integration with Stripe
- SouqBooks integrates with Stripe to securely handle online payments.
- Users can choose Stripe as the payment method during checkout.
![Payment Integration](https://i.postimg.cc/wvMFS5K3/Payment.png)

#### Search in Products for Users and Admins
- Users and admins can perform product searches to find specific items quickly.
- The search functionality helps users locate products based on their names or other attributes.

#### Search in Orders for Admins
- Admins can search for specific orders based on order numbers, customer names, or other relevant criteria.
- The search feature simplifies order management for administrators.

#### Display of Orders in Admin Panel
- The admin panel displays orders categorized by their status (Pending, In Progress, Shipped, and Cancelled).
- Each category shows the number of orders and their total price for quick reference.
