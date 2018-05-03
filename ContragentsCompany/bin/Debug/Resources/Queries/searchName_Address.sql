select CompanyName, RegionName, RayonName, CityName, StreetName, BuildNumb, Postcode
from Company inner join Region on Region.id = Address.id_Region 
inner join Rayon on Rayon.id = Address.id_Rayon 
inner join City on City.id = Address.id_City 
inner join Street on Street.id = Address.id_Street 
inner join Building on Building.id = Address.id_Building 
inner join Address on Company.id_Address = Address.id