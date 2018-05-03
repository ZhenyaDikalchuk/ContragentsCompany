select Region.RegionName, Rayon.RayonName, City.CityName, Street.StreetName, Building.BuildNumb, Building.Postcode 
from Address inner join Region on Region.id = Address.id_Region 
inner join Rayon on Rayon.id = Address.id_Rayon 
inner join City on City.id = Address.id_City 
inner join Street on Street.id = Address.id_Street 
inner join Building on Building.id = Address.id_Building 
inner join Company on Company.id_Address = Address.id
