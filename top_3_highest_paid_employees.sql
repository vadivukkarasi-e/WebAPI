SELECT "EmployeeID", "Name", "Department", "Salary"
FROM (
    SELECT 
        "EmployeeID", 
        "Name", 
        "Department", 
        "Salary",
        ROW_NUMBER() OVER (PARTITION BY "Department" ORDER BY "Salary" DESC) AS rank
    FROM public."Employees"
) AS ranked_employees
WHERE rank <= 3
ORDER BY "Department", rank;