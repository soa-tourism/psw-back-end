DELETE FROM stakeholders."People";
DELETE FROM stakeholders."Users";

DELETE FROM payments."TouristWallets";
DELETE FROM payments."ShoppingCarts";

-- password for all: nina
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Role", "IsActive", "IsVerified")
VALUES (-1, 'admin', 'a5c299e2fdd21869360a5e52cb764eafc7b94bd0eed0a2080c427684024242e0', 0, true, true);

INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Role", "IsActive", "IsVerified")
VALUES (-11, 'autor1', 'a5c299e2fdd21869360a5e52cb764eafc7b94bd0eed0a2080c427684024242e0', 1, true, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Role", "IsActive", "IsVerified")
VALUES (-12, 'autor2', 'a5c299e2fdd21869360a5e52cb764eafc7b94bd0eed0a2080c427684024242e0', 1, true, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Role", "IsActive", "IsVerified")
VALUES (-13, 'autor3', 'a5c299e2fdd21869360a5e52cb764eafc7b94bd0eed0a2080c427684024242e0', 1, true, true);

INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Role", "IsActive", "IsVerified")
VALUES (-21, 'turista1', 'a5c299e2fdd21869360a5e52cb764eafc7b94bd0eed0a2080c427684024242e0', 2, true, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Role", "IsActive", "IsVerified")
VALUES (-22, 'turista2', 'a5c299e2fdd21869360a5e52cb764eafc7b94bd0eed0a2080c427684024242e0', 2, true, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Role", "IsActive", "IsVerified")
VALUES (-23, 'turista3', 'a5c299e2fdd21869360a5e52cb764eafc7b94bd0eed0a2080c427684024242e0', 2, true, true);


INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "ProfilePictureUrl", "Biography", "Motto")
VALUES (-11, -11, 'Ana', 'Anić', 'autor1@gmail.com', 'default_url', 'Default Biography', 'Default Motto');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "ProfilePictureUrl", "Biography", "Motto")
VALUES (-12, -12, 'Lena', 'Lenić', 'autor2@gmail.com', 'default_url', 'Default Biography', 'Default Motto');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "ProfilePictureUrl", "Biography", "Motto")
VALUES (-13, -13, 'Sara', 'Sarić', 'autor3@gmail.com', 'default_url', 'Default Biography', 'Default Motto');

INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "ProfilePictureUrl", "Biography", "Motto")
VALUES (-21, -21, 'Pera', 'Perić', 'turista1@gmail.com', 'default_url', 'Default Biography', 'Default Motto');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "ProfilePictureUrl", "Biography", "Motto")
VALUES (-22, -22, 'Mika', 'Mikić', 'turista2@gmail.com', 'default_url', 'Default Biography', 'Default Motto');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "ProfilePictureUrl", "Biography", "Motto")
VALUES (-23, -23, 'Steva', 'Stević', 'turista3@gmail.com', 'default_url', 'Default Biography', 'Default Motto');



INSERT INTO payments."TouristWallets"(
    "Id", "AdventureCoins", "UserId")
VALUES (-11, 1000, -21);
INSERT INTO payments."TouristWallets"(
    "Id", "AdventureCoins", "UserId")
VALUES (-12, 1000, -22);
INSERT INTO payments."TouristWallets"(
    "Id", "AdventureCoins", "UserId")
VALUES (-13, 1000, -23);


INSERT INTO payments."ShoppingCarts" ("Id", "UserId", "Items", "LastActivity", "Changes", "Version")
VALUES (-11, -21, '[]', CURRENT_TIMESTAMP, '[]', 0);
INSERT INTO payments."ShoppingCarts" ("Id", "UserId", "Items", "LastActivity", "Changes", "Version")
VALUES (-12, -22, '[]', CURRENT_TIMESTAMP, '[]', 0);
INSERT INTO payments."ShoppingCarts" ("Id", "UserId", "Items", "LastActivity", "Changes", "Version")
VALUES (-13, -23, '[]', CURRENT_TIMESTAMP, '[]', 0);
