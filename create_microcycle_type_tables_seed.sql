-- =============================================
-- Script de creación de datos semilla para tipos de microciclo
-- Ejecutar después de la migración de EF Core
-- =============================================

-- Tipos de microciclo base (los IDs son GUIDs fijos para referencia)
INSERT INTO MicrocycleType (MicrocycleTypeId, Name, Description, Status, CreatedAt) VALUES
('mt-ordinario-0001', 'Ordinario', 'Microciclo de carga estándar con distribución equilibrada de entrenamiento', 1, NOW()),
('mt-choque-00002', 'Choque', 'Microciclo de alta intensidad y volumen elevado para generar adaptaciones', 1, NOW()),
('mt-activacion-003', 'Activación', 'Microciclo previo a competencia con carga moderada-alta para preparar al atleta', 1, NOW()),
('mt-competitivo-04', 'Competitivo', 'Microciclo durante periodo de competencia con énfasis en rendimiento', 1, NOW()),
('mt-recuperacion-5', 'Recuperación', 'Microciclo de descarga y regeneración post-competencia', 1, NOW()),
('mt-descarga-0006', 'Descarga', 'Microciclo con reducción progresiva de volumen para evitar sobreentrenamiento', 1, NOW()),
('mt-evaluacion-007', 'Evaluación', 'Microciclo dedicado a pruebas, valoraciones y tests de rendimiento', 1, NOW());

-- Porcentajes por defecto para "Ordinario"
INSERT INTO MicrocycleTypeDayDefault (MicrocycleTypeDayDefaultId, MicrocycleTypeId, DayOfWeek, ThrowPercentage) VALUES
(UUID(), 'mt-ordinario-0001', 'lunes', 15),
(UUID(), 'mt-ordinario-0001', 'martes', 20),
(UUID(), 'mt-ordinario-0001', 'miercoles', 20),
(UUID(), 'mt-ordinario-0001', 'jueves', 15),
(UUID(), 'mt-ordinario-0001', 'viernes', 15),
(UUID(), 'mt-ordinario-0001', 'sabado', 10),
(UUID(), 'mt-ordinario-0001', 'domingo', 5);

-- Porcentajes por defecto para "Choque"
INSERT INTO MicrocycleTypeDayDefault (MicrocycleTypeDayDefaultId, MicrocycleTypeId, DayOfWeek, ThrowPercentage) VALUES
(UUID(), 'mt-choque-00002', 'lunes', 20),
(UUID(), 'mt-choque-00002', 'martes', 25),
(UUID(), 'mt-choque-00002', 'miercoles', 20),
(UUID(), 'mt-choque-00002', 'jueves', 15),
(UUID(), 'mt-choque-00002', 'viernes', 10),
(UUID(), 'mt-choque-00002', 'sabado', 10),
(UUID(), 'mt-choque-00002', 'domingo', 0);

-- Porcentajes por defecto para "Activación"
INSERT INTO MicrocycleTypeDayDefault (MicrocycleTypeDayDefaultId, MicrocycleTypeId, DayOfWeek, ThrowPercentage) VALUES
(UUID(), 'mt-activacion-003', 'lunes', 15),
(UUID(), 'mt-activacion-003', 'martes', 20),
(UUID(), 'mt-activacion-003', 'miercoles', 20),
(UUID(), 'mt-activacion-003', 'jueves', 20),
(UUID(), 'mt-activacion-003', 'viernes', 15),
(UUID(), 'mt-activacion-003', 'sabado', 10),
(UUID(), 'mt-activacion-003', 'domingo', 0);

-- Porcentajes por defecto para "Competitivo"
INSERT INTO MicrocycleTypeDayDefault (MicrocycleTypeDayDefaultId, MicrocycleTypeId, DayOfWeek, ThrowPercentage) VALUES
(UUID(), 'mt-competitivo-04', 'lunes', 10),
(UUID(), 'mt-competitivo-04', 'martes', 15),
(UUID(), 'mt-competitivo-04', 'miercoles', 15),
(UUID(), 'mt-competitivo-04', 'jueves', 20),
(UUID(), 'mt-competitivo-04', 'viernes', 20),
(UUID(), 'mt-competitivo-04', 'sabado', 15),
(UUID(), 'mt-competitivo-04', 'domingo', 5);

-- Porcentajes por defecto para "Recuperación"
INSERT INTO MicrocycleTypeDayDefault (MicrocycleTypeDayDefaultId, MicrocycleTypeId, DayOfWeek, ThrowPercentage) VALUES
(UUID(), 'mt-recuperacion-5', 'lunes', 10),
(UUID(), 'mt-recuperacion-5', 'martes', 15),
(UUID(), 'mt-recuperacion-5', 'miercoles', 15),
(UUID(), 'mt-recuperacion-5', 'jueves', 15),
(UUID(), 'mt-recuperacion-5', 'viernes', 15),
(UUID(), 'mt-recuperacion-5', 'sabado', 15),
(UUID(), 'mt-recuperacion-5', 'domingo', 15);

-- Porcentajes por defecto para "Descarga"
INSERT INTO MicrocycleTypeDayDefault (MicrocycleTypeDayDefaultId, MicrocycleTypeId, DayOfWeek, ThrowPercentage) VALUES
(UUID(), 'mt-descarga-0006', 'lunes', 15),
(UUID(), 'mt-descarga-0006', 'martes', 15),
(UUID(), 'mt-descarga-0006', 'miercoles', 15),
(UUID(), 'mt-descarga-0006', 'jueves', 15),
(UUID(), 'mt-descarga-0006', 'viernes', 15),
(UUID(), 'mt-descarga-0006', 'sabado', 15),
(UUID(), 'mt-descarga-0006', 'domingo', 10);

-- Porcentajes por defecto para "Evaluación"
INSERT INTO MicrocycleTypeDayDefault (MicrocycleTypeDayDefaultId, MicrocycleTypeId, DayOfWeek, ThrowPercentage) VALUES
(UUID(), 'mt-evaluacion-007', 'lunes', 15),
(UUID(), 'mt-evaluacion-007', 'martes', 15),
(UUID(), 'mt-evaluacion-007', 'miercoles', 20),
(UUID(), 'mt-evaluacion-007', 'jueves', 20),
(UUID(), 'mt-evaluacion-007', 'viernes', 15),
(UUID(), 'mt-evaluacion-007', 'sabado', 10),
(UUID(), 'mt-evaluacion-007', 'domingo', 5);

