from flask import Flask, request, jsonify, send_from_directory
import os
from werkzeug.utils import secure_filename
from utils.face_detection import detect_face_shape

app = Flask(__name__)

# Yüklenen dosyaların kaydedileceği klasör
UPLOAD_FOLDER = 'uploads'
app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER

# Statik dosya yolu
app.config['STATIC_FOLDER'] = 'static'

if not os.path.exists(UPLOAD_FOLDER):
    os.makedirs(UPLOAD_FOLDER)

@app.route('/analyze', methods=['POST'])
def analyze_image():
    if 'file' not in request.files:
        return jsonify({"error": "Dosya yüklenmedi!"}), 400

    file = request.files['file']
    if file.filename == '':
        return jsonify({"error": "Dosya adı boş!"}), 400

    filename = secure_filename(file.filename)
    filepath = os.path.join(app.config['UPLOAD_FOLDER'], filename)
    file.save(filepath)

    # Yüz şekli analizi
    face_shape = detect_face_shape(filepath)
    if not face_shape:
        return jsonify({"error": "Yüz algılanamadı!"}), 400

    # Saç dosyasını seç
    hair_file = f"{face_shape}.png"
    hair_file_path = os.path.join(app.config['STATIC_FOLDER'], 'saclar', hair_file)

    if not os.path.exists(hair_file_path):
        return jsonify({"error": "Uygun saç bulunamadı!"}), 400

    # Saç dosyasının doğru yolu döndürülüyor
    return jsonify({"face_shape": face_shape, "suggestion": f"/static/saclar/{hair_file}"})

@app.route('/static/saclar/<filename>')
def send_hair_image(filename):
    try:
        return send_from_directory(os.path.join(app.config['STATIC_FOLDER'], 'saclar'), filename)
    except FileNotFoundError:
        return jsonify({"error": "Dosya bulunamadı!"}), 404

if __name__ == '__main__':
    app.run(debug=True)
